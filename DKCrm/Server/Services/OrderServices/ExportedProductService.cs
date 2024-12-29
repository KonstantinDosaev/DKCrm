using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

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

        public async Task<IEnumerable<ExportedProduct>> GetNotEquippedAsync(Guid productId)
        {
           
            return await _context.ExportedProducts.Where(w => w.ProductId == productId && (!w.ExportedOrder!.OrderIsOver || w.ExportedOrder == null))
                .Select(s => new ExportedProduct()
                {
                    Id = s.Id,
                    Product = s.Product,
                    Quantity = s.Quantity,
                    ExportedOrder = s.ExportedOrder,
                    ExportedOrderId = s.ExportedOrderId,
                    ProductId = s.ProductId,
                    ImportedProducts = s.ImportedProducts,
                    PurchaseAtExports = s.PurchaseAtExports,
                    SoldFromStorage = s.SoldFromStorage,
                    StorageList = s.StorageList,
                    MinDaysForDeliveryPlaned = s.MinDaysForDeliveryPlaned, 
                    MaxDaysForDeliveryPlaned = s.MaxDaysForDeliveryPlaned,
                    DateTimeConversionCurrency = s.DateTimeConversionCurrency
                }).ToListAsync();
        }
        public async Task<IEnumerable<ExportedProduct>> GetAllNotEquippedAsync()
        {

            return await _context.ExportedProducts.Where(w =>
                    w.SoldFromStorage!.Select(s=>s.Quantity).Sum() + w.PurchaseAtExports!.Select(s=>s.Quantity).Sum() < w.Quantity
                                                              && (!w.ExportedOrder!.OrderIsOver || w.ExportedOrder == null))
                .Select(s => new ExportedProduct()
                {
                    Id = s.Id,
                    Product = s.Product,
                    Quantity = s.Quantity,
                    ExportedOrder = s.ExportedOrder,
                    ExportedOrderId = s.ExportedOrderId,
                    ProductId = s.ProductId,
                    ImportedProducts = s.ImportedProducts,
                    PurchaseAtExports = s.PurchaseAtExports,
                    SoldFromStorage = s.SoldFromStorage,
                    StorageList = s.StorageList,
                    MinDaysForDeliveryPlaned = s.MinDaysForDeliveryPlaned,
                    MaxDaysForDeliveryPlaned = s.MaxDaysForDeliveryPlaned,
                    DateTimeConversionCurrency = s.DateTimeConversionCurrency
                }).ToListAsync();
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
        public async Task<SortPagedResponse<ExportedProduct>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterExportedProductTuple> request)
        {
            var data = _context.ExportedProducts.AsQueryable();
            if (request.FilterTuple is { IsNotComplete: true })
            {
                data = data.Where(w =>
                    w.SoldFromStorage!.Select(s => s.Quantity).Sum() +
                    w.PurchaseAtExports!.Select(s => s.Quantity).Sum() < w.Quantity
                    && (!w.ExportedOrder!.OrderIsOver || w.ExportedOrder == null));
            }
            data = data.Select(s => new ExportedProduct()
            {
                Id = s.Id,
                Product = s.Product,
                Quantity = s.Quantity,
                ExportedOrder = s.ExportedOrder,
                ExportedOrderId = s.ExportedOrderId,
                ProductId = s.ProductId,
                ImportedProducts = s.ImportedProducts,
                PurchaseAtExports = s.PurchaseAtExports,
                SoldFromStorage = s.SoldFromStorage,
                StorageList = s.StorageList,
                MinDaysForDeliveryPlaned = s.MinDaysForDeliveryPlaned,
                MaxDaysForDeliveryPlaned = s.MaxDaysForDeliveryPlaned,
                DateTimeConversionCurrency = s.DateTimeConversionCurrency
            });
           
            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.FilterOrderTuple != null)
                {
                    if (request.FilterTuple.FilterOrderTuple.CurrentPartnerCompanyId != null && request.FilterTuple.FilterOrderTuple.CurrentPartnerCompanyId != Guid.Empty)
                    {
                        data = data.Where(o =>
                            o.ExportedOrder!.CompanyBuyerId ==
                            request.FilterTuple.FilterOrderTuple.CurrentPartnerCompanyId);
                        if (request.FilterTuple.FilterOrderTuple.CurrentPartnerEmployeeId != null && request.FilterTuple.FilterOrderTuple.CurrentPartnerEmployeeId != Guid.Empty)
                        {
                            data = data.Where(o =>
                                o.ExportedOrder!.EmployeeBuyerId ==
                                request.FilterTuple.FilterOrderTuple.CurrentPartnerEmployeeId);
                        }
                    }

                    if (request.FilterTuple.FilterOrderTuple.CurrentOurCompanyId != null && request.FilterTuple.FilterOrderTuple.CurrentOurCompanyId != Guid.Empty)
                    {
                        data = data.Where(o =>
                            o.ExportedOrder!.OurCompanyId == request.FilterTuple.FilterOrderTuple.CurrentOurCompanyId);
                        if (request.FilterTuple.FilterOrderTuple.CurrentOurEmployeeId != null && request.FilterTuple.FilterOrderTuple.CurrentOurEmployeeId != Guid.Empty)
                        {
                            data = data.Where(o =>
                                o.ExportedOrder!.OurEmployeeId ==
                                request.FilterTuple.FilterOrderTuple.CurrentOurEmployeeId);
                        }
                    }
                    if (request.FilterTuple.FilterOrderTuple.CreateDateFirst != null)
                    {
                        data = data.Where(w => w.ExportedOrder!.DateTimeCreated!.Value.Date >= request.FilterTuple.FilterOrderTuple.CreateDateFirst.Value.Date);
                    }
                    if (request.FilterTuple.FilterOrderTuple.CreateDateLast != null)
                    {
                        data = data.Where(w => w.ExportedOrder!.DateTimeCreated!.Value.Date <= request.FilterTuple.FilterOrderTuple.CreateDateLast.Value.Date);
                    }
                }
            }
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                if (request.SearchInChapter != null)
                {
                    switch (request.SearchInChapter)
                    {
                        case SearchChapterNames.ProductPartNumber:
                            {
                                data = data.Where(w => w.Product!.PartNumber!.ToLower().Contains(request.SearchString.ToLower()));
                                break;
                            }
                        case SearchChapterNames.CompanyName:
                            data = data.Where(w =>
                                w.ExportedOrder != null && (w.ExportedOrder.OurCompany != null && w.ExportedOrder.OurCompany.Name.ToLower().Contains(request.SearchString.ToLower()) ||
                                                            w.ExportedOrder.CompanyBuyer != null && w.ExportedOrder.CompanyBuyer.Name.ToLower().Contains(request.SearchString.ToLower())));
                            break;
                    }
                }
            }

            var totalItems = data.Count();

            switch (request.SortLabel)
            {
                case "create_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.ExportedOrder!.DateTimeCreated);
                    break;
                case "update_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeUpdate);
                    break;
            }

            List<ExportedProduct> result;
            if (request.FilterTuple is { GroupByOrder: true })
            {
                var groupCollection = data.Select(s => s.ExportedOrderId).Distinct();
                totalItems = groupCollection.Count();
                var pagination = groupCollection.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);
                result = await data.Where(w=> pagination.Contains(w.ExportedOrderId)).AsSingleQuery().ToListAsync();
                await _context.ExportedOrders.Where(w => result!.Select(s => s.ExportedOrderId).Contains(w.Id))
                    .Include(i=>i.CompanyBuyer)
                    .Include(i=>i.OurCompany).LoadAsync();
            }
            else
            {
                data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);
                result = await data.AsSingleQuery().ToListAsync();
            }
            await _context.Products.Where(w => result!.Select(s => s.ProductId).Contains(w.Id)).Include(i => i.Brand).LoadAsync();
            return new SortPagedResponse<ExportedProduct>() { TotalItems = totalItems, Items = result};
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
