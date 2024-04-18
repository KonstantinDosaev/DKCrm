using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Requests;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.OrderServices
{
    public class ImportedProductService : IImportedProductService
    {
        private readonly ApplicationDBContext _context;

        public ImportedProductService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ImportedProduct>> GetAsync()
        {
            return await _context.ImportedProducts
                .Include(i => i.ImportedOrder)
                .Include(i => i.Product).IgnoreQueryFilters().AsSplitQuery().ToListAsync();
        }
        
        public async Task<ImportedProduct> GetOneAsync(Guid id)
        {
            return await _context.ImportedProducts
                .Include(i => i.ImportedOrder)
                .Include(i=>i.PurchaseAtStorageList)
                .Include(i => i.PurchaseAtExportList)
                .Include(i => i.Product).ThenInclude(t => t!.ProductsInStorage)!.ThenInclude(t=>t.Storage).IgnoreQueryFilters().AsSingleQuery()
                .FirstOrDefaultAsync(a => a.Id == id) ?? throw new InvalidOperationException();
        }

        public async Task<IEnumerable<ImportedProduct>> GetNotEquippedAsync(Guid productId)
        {
            var statusImportedOrder = _context.ExportedOrderStatus.FirstOrDefault(f => f.Value == ImportOrderStatusNames.Completed)!.Position;
            return await _context.ImportedProducts.Where(w => w.ProductId == productId && (!w.ImportedOrder!.OrderIsOver || w.ImportedOrder == null))
                .Select(s => new ImportedProduct
                {
                    Id = s.Id,
                    Product = s.Product,
                    Quantity = s.Quantity,
                    ImportedOrder = s.ImportedOrder,
                    ImportedOrderId = s.ImportedOrderId,
                    ProductId = s.ProductId,
                    ExportedProducts = s.ExportedProducts,
                    PurchaseAtExportList = s.PurchaseAtExportList,
                    PurchaseAtStorageList = s.PurchaseAtStorageList,
                    StorageList = s.StorageList
                }).ToListAsync();
        }

        public async Task<IEnumerable<ImportedProduct>> GetAllNotAddToOrderAsync()
        {
            var importedProducts = await _context.ImportedProducts
                .Include(i=>i.ExportedProducts)!
                .ThenInclude(t=>t.ExportedOrder).Where(w => w.ImportedOrderId == null || w.ImportedOrderId == Guid.Empty)
                .Select(s => new ImportedProduct
                {
                    Id = s.Id,
                    Product = s.Product,
                    Quantity = s.Quantity,
                    ImportedOrder = s.ImportedOrder,
                    ImportedOrderId = s.ImportedOrderId,
                    ProductId = s.ProductId,
                    ExportedProducts = s.ExportedProducts,
                    PurchaseAtExportList = s.PurchaseAtExportList,
                    PurchaseAtStorageList = s.PurchaseAtStorageList,
                    StorageList = s.StorageList
                }).ToListAsync();
            await _context.Brands.Where(w => importedProducts.Select(s => s.Product!.BrandId).Contains(w.Id)).LoadAsync();
            
            return importedProducts;
        }
        public async Task<Guid> PostAsync(ImportedProduct importedProduct)
        {
            _context.Entry(importedProduct).State = EntityState.Added;
            if (importedProduct.PurchaseAtStorageList != null)
            {
                foreach (var item in importedProduct.PurchaseAtStorageList)
                {
                    _context.Entry(item).State = EntityState.Added;
                }
            }
            if (importedProduct.PurchaseAtExportList != null)
            {
                foreach (var item in importedProduct.PurchaseAtExportList)
                {
                    _context.Entry(item).State = EntityState.Added;
                }
            }
            await _context.SaveChangesAsync();
            return importedProduct.Id;
        }
        //[HttpPost("UpdatePurchaseAtExport")]
        //public async Task<IActionResult> UpdatePurchaseAtExport(PurchaseAtExport purchaseAtExport)
        //{
        //    var importedProduct = _context.ImportedProducts
        //        .Include(i=>i.PurchaseAtExportList)
        //        .Include(i=>i.PurchaseAtStorageList)
        //        .FirstOrDefault(i => i.Id == purchaseAtExport.ImportedProductId);

        //    var purchaseAtExportInDb = importedProduct.PurchaseAtExportList.FirstOrDefault(f =>
        //            f.ExportedProductId == purchaseAtExport.ExportedProductId && f.ImportedProductId == purchaseAtExport.ImportedProductId);
        //    var purchaseAtStorageInDb = importedProduct.PurchaseAtStorageList.FirstOrDefault(f =>
        //         f.ImportedProductId == purchaseAtExport.ImportedProductId);
        //    if (importedProduct != null)
        //    {
        //        if (purchaseAtExportInDb != null)
        //        {
        //            if (purchaseAtExportInDb.Quantity == purchaseAtExport.Quantity)
        //                return BadRequest();

        //            int modQuantity;
        //            if (purchaseAtExportInDb.Quantity < purchaseAtExport.Quantity)
        //            {
        //                modQuantity = purchaseAtExport.Quantity - purchaseAtExportInDb.Quantity;
        //                importedProduct.Quantity -= modQuantity;
        //            }
        //            else
        //            {
        //                modQuantity = purchaseAtExportInDb.Quantity - purchaseAtExport.Quantity;
        //                importedProduct.Quantity += modQuantity;
        //            }
        //        }
        //        else
        //        {
        //            importedProduct.Quantity -= soldFromStorage.Quantity;
        //        }

        //        if (importedProduct.Quantity < 0)
        //            return BadRequest();

        //        _context.Entry(importedProduct).State = EntityState.Modified;
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //    _context.Entry(soldFromStorage).State = purchaseAtExportInDb != null ? EntityState.Modified : EntityState.Added;

        //    await _context.SaveChangesAsync();
        //    return Ok(soldFromStorage.ExportedProductId);
        //}

        public async Task<int> UpdateSourcesOrderItems(ImportedProduct importedProduct)
        {
            var exportedProductInDb = await GetOneAsync(importedProduct.Id);
            if (importedProduct.PurchaseAtStorageList != null)
            {
                foreach (var purchaseAtStorage in importedProduct.PurchaseAtStorageList)
                {
                    var soldInDb = exportedProductInDb.PurchaseAtStorageList?.FirstOrDefault(f =>
                        f.ImportedProductId == purchaseAtStorage.ImportedProductId && f.StorageId == purchaseAtStorage.StorageId);

                    
                    if (soldInDb != null)
                    {
                        if (soldInDb.Quantity == purchaseAtStorage.Quantity)
                            continue;
                    }
                        
                    if (purchaseAtStorage.Quantity == 0)
                    {
                        _context.Entry(purchaseAtStorage).State = EntityState.Deleted;
                        continue;
                    }
                    _context.Entry(purchaseAtStorage).State = soldInDb != null ? EntityState.Modified : EntityState.Added;
                }

            }

            if (importedProduct.PurchaseAtExportList != null)
            {
                foreach (var purchaseAtExport in importedProduct.PurchaseAtExportList)
                {
                    var purchaseAtExportInDb = exportedProductInDb.PurchaseAtExportList?.FirstOrDefault(f =>
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
                        if (purchaseAtExport.Quantity != 0)
                            _context.Entry(purchaseAtExport).State = EntityState.Added;
                    }
                }
            }

            return await _context.SaveChangesAsync();
        }
        public async Task<Guid> MergeImportedProductsAsync(MergeImportedProductsRequest mergeRequest)
        {
            var primaryImportedProduct = await GetOneAsync(mergeRequest.PrimaryImportedProductId);
            var secondaryImportedProduct = await GetOneAsync(mergeRequest.SecondaryImportedProductId);

            var purchaseAtExportListSecondaryInDb =
               _context.PurchaseAtExports.Where(s => s.ImportedProductId == secondaryImportedProduct.Id).AsNoTracking().ToList();
            //var purchaseAtExportListSecondaryInDb =
            //   secondaryImportedProduct.PurchaseAtExportList;


            if (purchaseAtExportListSecondaryInDb != null && purchaseAtExportListSecondaryInDb.Any())
            {
                primaryImportedProduct.PurchaseAtExportList ??= new List<PurchaseAtExport>();
                foreach (var purchaseAtExport in purchaseAtExportListSecondaryInDb)
                {
                   secondaryImportedProduct.PurchaseAtExportList!.Remove(secondaryImportedProduct.PurchaseAtExportList.FirstOrDefault(f=>f.ExportedProductId == purchaseAtExport.ExportedProductId)!);
                   await _context.SaveChangesAsync();
                    //primaryImportedProduct.PurchaseAtExportList.Add(purchaseAtExport);
                    primaryImportedProduct.PurchaseAtExportList.Add(purchaseAtExport);
                    _context.ChangeTracker.DetectChanges();
                }
            }
            primaryImportedProduct.Quantity += secondaryImportedProduct.Quantity;
            //_context.Entry(primaryImportedProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _context.Entry(secondaryImportedProduct).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return mergeRequest.PrimaryImportedProductId;
        }

        public async Task<Guid> PutAsync(ImportedProduct importedProduct)
        {
            _context.Entry(importedProduct).State = EntityState.Modified;

            if (importedProduct.PurchaseAtStorageList != null)
            {
                var purchaseAtStorageInDb = await _context.PurchaseAtStorages.Where(w => w.ImportedProductId == importedProduct.Id).Select(s => s.StorageId).ToListAsync();

                foreach (var item in importedProduct.PurchaseAtStorageList)
                {
                    _context.Entry(item).State = purchaseAtStorageInDb.Contains(item.StorageId) ? EntityState.Modified : EntityState.Added;
                }
            }
            if (importedProduct.PurchaseAtExportList != null)
            {
                var purchaseAtExportInDb = await _context.PurchaseAtExports.Where(w => w.ImportedProductId == importedProduct.Id).Select(s => s.ExportedProductId).ToListAsync();

                foreach (var item in importedProduct.PurchaseAtExportList)
                {
                    _context.Entry(item).State = purchaseAtExportInDb.Contains(item.ExportedProductId) ? EntityState.Modified : EntityState.Added;
                }
            }

            await _context.SaveChangesAsync();
            return importedProduct.Id;
        }
        public async Task<IEnumerable<Guid>> PutRangeAsync(IEnumerable<ImportedProduct> importedProducts)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.ImportedProducts.UpdateRange(importedProducts);
            await _context.SaveChangesAsync();
            return importedProducts.Select(s=>s.Id);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = await GetOneAsync(id);
            _context.Remove(dev);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<ImportedProduct> importedProducts)
        {
            _context.RemoveRange(importedProducts);
            return await _context.SaveChangesAsync();
        }
    }
}
