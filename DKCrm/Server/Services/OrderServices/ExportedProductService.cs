using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.OrderServices
{
    public class ExportedProductService : IExportedProductService
    {
        private readonly ApplicationDBContext _context;

        public ExportedProductService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExportedProduct>> GetAsync()
        {
            return await _context.ExportedProducts.Include(i => i.ExportedOrder).ToListAsync();
        }

        public async Task<IEnumerable<ExportedProduct>> GetNotEquippedAsync()
        {
            var status = _context.ExportedOrderStatus.FirstOrDefault(f => f.Value == ExportOrderStatusNames.Delivery)!.Position;
            return await _context.ExportedProducts.Select(s => new ExportedProduct
            {
                Product = s.Product,
                Quantity = s.Quantity,
                ExportedOrder = s.ExportedOrder,
                ExportedOrderId = s.ExportedOrderId,
                ProductId = s.ProductId,
                ImportedProducts = s.ImportedProducts,
                PurchaseAtExports = s.PurchaseAtExports,
                SoldFromStorage = s.SoldFromStorage,
                StorageList = s.StorageList
            })
                .Where(w => w.Quantity < w.SoldFromStorage!.Select(s => s.Quantity).Sum() + w.PurchaseAtExports!.Select(s => s.Quantity).Sum())
                .ToListAsync();
        }

        public async Task<ExportedProduct> GetOneAsync(Guid id)
        {
            var dev = await _context.ExportedProducts
                .Include(i => i.ExportedOrder)
                .Include(i => i.SoldFromStorage)
                .Include(i => i.PurchaseAtExports)
                .Include(i => i.Product).ThenInclude(t => t!.Brand).IgnoreQueryFilters()
                .Include(i => i.Product!.ProductsInStorage!).ThenInclude(t => t.Storage).AsSingleQuery().AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            return dev ?? throw new InvalidOperationException();
        }

        public async Task<Guid> PostAsync(ExportedProduct exportedProduct)
        {
            _context.Entry(exportedProduct).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return exportedProduct.Id;
        }


        public async Task<Guid> PutAsync(ExportedProduct exportedProduct)
        {
            _context.Entry(exportedProduct).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
            return exportedProduct.Id;
        }
        public async Task<int> UpdateSourcesOrderItems( ExportedProduct exportedProduct)
        {
            var exportedProductInDb = await GetOneAsync(exportedProduct.Id);
            if (exportedProduct.SoldFromStorage != null)
            {
                foreach (var soldFromStorage in exportedProduct.SoldFromStorage)
                {
                    var productInStorage = exportedProduct.Product?.ProductsInStorage?.FirstOrDefault(i =>
                        i.ProductId == exportedProduct!.ProductId
                        && i.StorageId == soldFromStorage.StorageId);

                    var soldInDb = exportedProductInDb.SoldFromStorage?.FirstOrDefault(f =>
                        f.ExportedProductId == soldFromStorage.ExportedProductId && f.StorageId == soldFromStorage.StorageId);

                    if (productInStorage != null)
                    {
                        if (soldInDb != null)
                        {
                            if (soldInDb.Quantity == soldFromStorage.Quantity)
                                continue;

                            int modQuantity;
                            if (soldInDb.Quantity < soldFromStorage.Quantity)
                            {
                                modQuantity = soldFromStorage.Quantity - soldInDb.Quantity;
                                productInStorage.Quantity -= modQuantity;
                            }
                            else
                            {
                                modQuantity = soldInDb.Quantity - soldFromStorage.Quantity;
                                productInStorage.Quantity += modQuantity;
                            }
                        }
                        else
                        {
                            productInStorage.Quantity -= soldFromStorage.Quantity;
                        }

                        if (productInStorage.Quantity < 0)
                            return 0;

                        _context.Entry(productInStorage).State = EntityState.Modified;
                    }
                    else
                    {
                        return 0;
                    }

                    if (soldFromStorage.Quantity == 0)
                    {
                        _context.Entry(soldFromStorage).State = EntityState.Deleted;
                        continue;
                    }
                    _context.Entry(soldFromStorage).State = soldInDb != null ? EntityState.Modified : EntityState.Added;
                }
               
            }

            if (exportedProduct.PurchaseAtExports != null)
            {
                foreach (var purchaseAtExport in exportedProduct.PurchaseAtExports)
                {
                    var purchaseAtExportInDb = exportedProductInDb.PurchaseAtExports?.FirstOrDefault(f =>
                        f.ExportedProductId == purchaseAtExport.ExportedProductId && f.ImportedProductId == purchaseAtExport.ImportedProductId);
                    if (purchaseAtExportInDb != null)
                    {
                        if (purchaseAtExportInDb.Quantity == purchaseAtExport.Quantity)
                        {
                            continue;
                        }
                        if (purchaseAtExport.Quantity == 0)
                        {
                            _context.Entry(purchaseAtExport).State = EntityState.Deleted;
                            continue;
                        }
                        _context.Entry(purchaseAtExport).State = EntityState.Modified;
                    }
                    else
                    {
                        if (purchaseAtExport.Quantity!= 0)
                            _context.Entry(purchaseAtExport).State =  EntityState.Added;
                    }
                }
            }

            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            var item = await _context.ExportedProducts.FirstOrDefaultAsync(a => a.Id == id);
            if (item == null) return 0;
            _context.Remove(item);
            return await _context.SaveChangesAsync();
        }


        //public async Task<IActionResult> DeleteRange(IEnumerable<ExportedProduct> exportedProducts)
        //{
        //    _context.RemoveRange(exportedProducts);
        //    await _context.SaveChangesAsync();
        //    return Ok(exportedProducts.Count());
        //}
    }
}
