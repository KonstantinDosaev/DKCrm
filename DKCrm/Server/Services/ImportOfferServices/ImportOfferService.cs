using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DKCrm.Server.Interfaces.ImportOfferInterfaces;
using DKCrm.Shared.Models.OfferModels;
using MudBlazor;


namespace DKCrm.Server.Services.ImportOfferServices
{
    public class ImportOfferService : IImportOfferService
    {
        private readonly ApplicationDBContext _context;

        public ImportOfferService(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<ImportOffer> GetDetailAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value)
                .FirstOrDefault();
            if (userId == null) return new ImportOffer();
            var accessCollection = _context.AccessRestrictions;
            var accessedComponents = accessCollection.Select(s => s.AccessedComponentId);
            var offer = await _context.ImportOffers.Where(w => (
                (w.CompanyId != null && (!accessedComponents.Contains((Guid)w.CompanyId) || (
                    accessedComponents.Contains((Guid)w.CompanyId)
                    && accessCollection.First(f => f.AccessedComponentId == w.CompanyId).AccessUsersToComponent
                        .Contains(userId)))))).Select(s => new ImportOffer()
                        {
                            Id = s.Id,
                            Quantity = s.Quantity,
                            Company = s.Company,
                            CompanyId = s.CompanyId,
                            Product = s.Product,
                            ProductId = s.ProductId,
                            PricesForImportOffer = s.PricesForImportOffer,

                            DateTimeCreate = s.DateTimeUpdate,
                            DateTimeUpdate = s.DateTimeUpdate,
                            IsDeleted = s.IsDeleted,
                            IsFullDeleted = s.IsFullDeleted
                        })
                .Include(i=>i.PricesForImportOffer)!
                .ThenInclude(t=>t.ExportProductPriceImportOffers)
                .FirstOrDefaultAsync(a => a.Id == id);
            /*await _context.PurchaseAtStorages
                .Where(w => order!.ImportedProducts!.Select(s => s.Id).Contains(w.ImportedProductId)).LoadAsync();
            await _context.PurchaseAtExports
                .Where(w => order!.ImportedProducts!.Select(s => s.Id).Contains(w.ImportedProductId)).LoadAsync();
            await _context.Products.Where(w => order!.ImportedProducts!.Select(s => s.ProductId).Contains(w.Id))
                .Include(i => i.Brand).LoadAsync();*/
            return offer ?? throw new InvalidOperationException();
        }

        public async Task<SortPagedResponse<ImportOffer>> GetBySortPagedSearchChapterAsync(
            SortPagedRequest<FilterOfferTuple> request, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value)
                .FirstOrDefault();
            if (userId == null) return new SortPagedResponse<ImportOffer>();
            var accessCollection = _context.AccessRestrictions;
            var accessedComponents = accessCollection.Select(s => s.AccessedComponentId);
            var data = _context.ImportOffers.Select(
                s => new ImportOffer()
                {
                    Id = s.Id,
                    Quantity = s.Quantity,
                    Company = s.Company,
                    CompanyId = s.CompanyId,
                    Product = s.Product,
                    ProductId = s.ProductId,
   
                    PricesForImportOffer = s.PricesForImportOffer,
                    
                    DateTimeCreate = s.DateTimeUpdate,
                    DateTimeUpdate = s.DateTimeUpdate,
                    IsDeleted = s.IsDeleted,
                    IsFullDeleted = s.IsFullDeleted
                });
       
            data = data.Where(w => (
                (w.CompanyId != null && (!accessedComponents.Contains((Guid)w.CompanyId) || (
                    accessedComponents.Contains((Guid)w.CompanyId)
                    && accessCollection.First(f => f.AccessedComponentId == w.CompanyId).AccessUsersToComponent
                        .Contains(userId))))));
            if (!string.IsNullOrEmpty(request.GlobalFilterValue))
            {
                switch (request.GlobalFilterType)
                {
                    case GlobalFilterTypes.Company:
                        data = data.Where(w =>
                            w.Company != null &&
                            w.Company.Name.ToLower().Contains(request.GlobalFilterValue.ToLower()));
                        break;
                    case GlobalFilterTypes.Product:
                        data = data.Where(w =>
                            w.Product != null &&
                            w.Product.PartNumber!.ToLower().Contains(request.GlobalFilterValue.ToLower()));
                        break;
                }
            }
           
            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.LoadOnlyNotOver)
                {
                    data = data.Where(w =>
                        w.PricesForImportOffer!.OrderBy(o => o.DateTimeOver).FirstOrDefault()!.DateTimeOver >=
                        DateTime.Now);
                }
                if (request.FilterTuple.ProductId != null &&
                    request.FilterTuple.ProductId != Guid.Empty)
                {
                    data = data.Where(o => o.ProductId == request.FilterTuple.ProductId);
                }
                if (request.FilterTuple.CurrentCompanyId != null &&
                    request.FilterTuple.CurrentCompanyId != Guid.Empty)
                {
                    data = data.Where(o => o.CompanyId == request.FilterTuple.CurrentCompanyId);
                }
                
                if (request.FilterTuple.CreateDateFirst != null)
                {
                    data = data.Where(w =>
                        w.DateTimeCreate!.Value.Date >= request.FilterTuple.CreateDateFirst.Value.Date);
                }

                if (request.FilterTuple.CreateDateLast != null)
                {
                    data = data.Where(w =>
                        w.DateTimeCreate!.Value.Date <= request.FilterTuple.CreateDateLast.Value.Date);
                }

                if (request.FilterTuple.UpdateDateFirst != null)
                {
                    data = data.Where(w =>
                        w.DateTimeUpdate!.Value.Date >= request.FilterTuple.UpdateDateFirst.Value.Date);
                }
                if (request.FilterTuple.UpdateDateLast != null)
                {
                    data = data.Where(w => w.DateTimeUpdate!.Value.Date <= request.FilterTuple.UpdateDateLast.Value.Date);
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
                                data = data.Where(w =>
                                    w.Product != null && w.Product.PartNumber!.ToLower().Contains(request.SearchString.ToLower()));
                                break;
                            }
                        case SearchChapterNames.CompanyName:
                            data = data.Where(w =>
                                w.Company != null &&
                                w.Company.Name.ToLower().Contains(request.SearchString.ToLower()));
                            break;
                    }
                }
            }

            var totalItems = data.Count();

            switch (request.SortLabel)
            {
                case "company_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Company!.Name);
                    break;
                
                case "create_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeCreate);
                    break;
                case "update_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeUpdate);
                    break;
                case "brand_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Product.Brand.Name);
                    break;
                
                case "quantity_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeUpdate);
                    break;
            }

            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);
            await _context.Products.Where(w => data!.Select(s => s.ProductId).Contains(w.Id))
                .Include(i => i.Brand).LoadAsync();
            await _context.ExportProductPriceImportOffers.Where(w => data!.SelectMany(s => s.PricesForImportOffer)
                    .Select(s=>s.Id).Contains(w.PriceId))
                .Include(i => i.ExportedProduct).LoadAsync();
            return new SortPagedResponse<ImportOffer>()
            {
                TotalItems = totalItems,
                Items = await data.AsSingleQuery().ToListAsync(),
                //AnyUnreadComment = unreadAny
            };
        }

        public async Task<Guid> PostAsync(ImportOffer offer)
        {
            offer.DateTimeCreate = DateTime.Now;
            offer.DateTimeUpdate = DateTime.Now;

          //  try
           // {
                _context.ImportOffers.Add(offer);
                await _context.SaveChangesAsync();
           // }
            /*catch 
            {
                _context.Entry(offer).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }*/
           
            /*var price = _context.PricesForImportOffers.OrderByDescending(o=>o.DateTimeOver)
                .FirstOrDefault(f => f.ImportOfferId == importedOrder.Id);
            if (price != null)
            {
                importedOrder.ImportedOrderStatus ??= new List<ImportedOrderStatus>();
                importedOrder.ImportedOrderStatus.Add(status);
            }

            if (importedOrder.ImportedProducts != null)
            {
                foreach (var importedProduct in importedOrder.ImportedProducts)
                {
                    _context.Entry(importedProduct).State = EntityState.Added;

                    if (importedProduct.PurchaseAtExportList != null)
                        foreach (var purchaseAtExport in importedProduct.PurchaseAtExportList)
                        {
                            _context.Entry(purchaseAtExport).State = EntityState.Added;
                        }
                }
            }*/


            return offer.Id;
        }
        public async Task<Guid> UpdateAsync(ImportOffer offer)
        {
            offer.DateTimeUpdate = DateTime.Now;

            //  try
            // {
            _context.Entry(offer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            // }
            /*catch
            {
                _context.Entry(offer).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }*/

            /*var price = _context.PricesForImportOffers.OrderByDescending(o=>o.DateTimeOver)
                .FirstOrDefault(f => f.ImportOfferId == importedOrder.Id);
            if (price != null)
            {
                importedOrder.ImportedOrderStatus ??= new List<ImportedOrderStatus>();
                importedOrder.ImportedOrderStatus.Add(status);
            }

            if (importedOrder.ImportedProducts != null)
            {
                foreach (var importedProduct in importedOrder.ImportedProducts)
                {
                    _context.Entry(importedProduct).State = EntityState.Added;

                    if (importedProduct.PurchaseAtExportList != null)
                        foreach (var purchaseAtExport in importedProduct.PurchaseAtExportList)
                        {
                            _context.Entry(purchaseAtExport).State = EntityState.Added;
                        }
                }
            }*/


            return offer.Id;
        }

        public async Task<int> UpdatePriceAsync(PriceForImportOffer newPrice)
        {
            var oldPrice = await _context.PricesForImportOffers.Where(f =>
                    f.ImportOfferId == newPrice.ImportOfferId).OrderByDescending(o=>o.DateTimeOver).FirstOrDefaultAsync();
            if (oldPrice != null)
            {
                oldPrice.DateTimeOver = DateTime.Now;
                oldPrice.DateTimeUpdate = DateTime.Now;
            }
            newPrice.DateTimeCreate = DateTime.Now;
            newPrice.DateTimeUpdate = DateTime.Now;
            newPrice.DateTimeOver = DateTime.MaxValue;
            _context.Entry(newPrice).State = EntityState.Added;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = await _context.ImportOffers.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return 0;

            _context.Entry(dev).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddOfferToExportOrderAsync(ExportProductPriceImportOffer link)
        {
            var ex = _context.PricesForImportOffers
                    .Include(i=>i.ExportProductPriceImportOffers)
                    .FirstOrDefault(f => f.Id == link.PriceId);
            var linkInDb = ex.ExportProductPriceImportOffers
                .FirstOrDefault(f => f.PriceId == link.PriceId 
                                     && link.ExportedProductId == f.ExportedProductId);
            if (ex == null)
                return 0;
            if (ex is { ExportProductPriceImportOffers: not null })
            {
                if (ex.Quantity < link.Quantity) return 0;
                var sum = ex.ExportProductPriceImportOffers.Sum(s => s.Quantity);
                if (ex.Quantity - sum < link.Quantity && linkInDb == null)
                    return 0;
            }

            if (ex.ExportProductPriceImportOffers != null)
            {
                if (linkInDb != null)
                {
                    linkInDb.Quantity = link.Quantity;
                    if (link.Quantity == 0)
                    {
                        _context.Entry(linkInDb).State = EntityState.Deleted;
                    }
                }
                else
                    _context.Entry(link).State = EntityState.Added;
            }

            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateOfferItemsSources(ExportedProduct exportedProduct)
        {
            /*var exportedProductInDb = await GetOneAsync(exportedProduct.Id);
            if (exportedProduct.SoldFromStorage != null)
            {
                foreach (var soldFromStorage in exportedProduct.SoldFromStorage)
                {
                    var productInStorage = exportedProduct.Product?.ProductsInStorage?.FirstOrDefault(i =>
                        i.ProductId == exportedProduct.ProductId
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
                        if (purchaseAtExport.Quantity != 0)
                            _context.Entry(purchaseAtExport).State = EntityState.Added;
                    }
                }
            }*/

            return await _context.SaveChangesAsync();
        }

    }
}
