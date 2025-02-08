using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System.Security.Claims;

namespace DKCrm.Server.Services.OrderServices
{
    public class ImportedOrderService : IImportedOrderService
    {
        private readonly ApplicationDBContext _context;
        private readonly IImportedProductService _importedProductService;
        private readonly IOrderCommentsService _orderCommentsService;
        private readonly IExportedOrderService _exportedOrderService;

        public ImportedOrderService(ApplicationDBContext context, IImportedProductService importedProductService,
            IOrderCommentsService orderCommentsService, IExportedOrderService exportedOrderService)
        {
            _context = context;
            _importedProductService = importedProductService;
            _orderCommentsService = orderCommentsService;
            _exportedOrderService = exportedOrderService;
        }

        public async Task<IEnumerable<ImportedOrder>> GetAsync(ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value)
                .FirstOrDefault();
            if (userId == null) return Array.Empty<ImportedOrder>();
            var accessCollection = _context.AccessRestrictions;
            var accessedComponents = accessCollection.Select(s => s.AccessedComponentId);
            await _context.Companies
                .Where(w => w.ImportedOrdersOurCompany!.Any() || w.ImportedOrdersSellersCompany!.Any())
                .Include(i => i.Employees).LoadAsync();
            return await _context.ImportedOrders.Where(w => (
                (w.SellersCompanyId != null && (!accessedComponents.Contains((Guid)w.SellersCompanyId) || (
                    accessedComponents.Contains((Guid)w.SellersCompanyId)
                    && accessCollection.First(f => f.AccessedComponentId == w.SellersCompanyId).AccessUsersToComponent
                        .Contains(userId)))))).ToListAsync();
        }

        public async Task<ImportedOrder> GetDetailAsync(Guid id, ClaimsPrincipal user)
        {
            //return await _context.ImportedOrders
            //    .Include(i => i.ImportedProducts!).ThenInclude(t => t.Product).ThenInclude(t => t!.Brand).IgnoreQueryFilters()
            //    .Include(f => f.OurCompany)
            //    .Include(i => i.SellersCompany)
            //    .Include(i => i.OurEmployee)
            //    .Include(i => i.EmployeeSeller).AsSingleQuery()
            //    .FirstOrDefaultAsync(a => a.Id == id) ?? throw new InvalidOperationException();
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value)
                .FirstOrDefault();
            if (userId == null) return new ImportedOrder();
            var accessCollection = _context.AccessRestrictions;
            var accessedComponents = accessCollection.Select(s => s.AccessedComponentId);
            var order = await _context.ImportedOrders.Where(w => (
                (w.SellersCompanyId != null && (!accessedComponents.Contains((Guid)w.SellersCompanyId) || (
                    accessedComponents.Contains((Guid)w.SellersCompanyId)
                    && accessCollection.First(f => f.AccessedComponentId == w.SellersCompanyId).AccessUsersToComponent
                        .Contains(userId)))))).Select(s => new ImportedOrder()
            {
                Id = s.Id,
                Number = s.Number,
                OurCompany = s.OurCompany,
                SellersCompany = s.SellersCompany,
                EmployeeSeller = s.EmployeeSeller,
                OurEmployee = s.OurEmployee,
                EmployeeSellerId = s.EmployeeSellerId,
                OurEmployeeId = s.OurEmployeeId,
                DateTimeCreated = s.DateTimeCreated,
                DateTimeUpdate = s.DateTimeUpdate,
                ImportedProducts = s.ImportedProducts,
                TransactionCurrency = s.TransactionCurrency,
                SupplierCurrency = s.SupplierCurrency,
                LocalCurrency = s.LocalCurrency,
                CurrencyPercent = s.CurrencyPercent,
                Nds = s.Nds,
                OrderIsWarn = s.OrderIsWarn,
                
                OrderIsOver = s.OrderIsOver,
                OrderIsLock = s.OrderIsLock,
                IsAllProductsAreCollected = s.IsAllProductsAreCollected,
                IsDeleted = s.IsDeleted,
                IsFullDeleted = s.IsFullDeleted,
                ImportedOrderStatusImportedOrders = s.ImportedOrderStatusImportedOrders,
                ImportedOrderStatus = s.ImportedOrderStatus,
                
                
                
            }).FirstOrDefaultAsync(a => a.Id == id);
            await _context.PurchaseAtStorages
                .Where(w => order!.ImportedProducts!.Select(s => s.Id).Contains(w.ImportedProductId)).LoadAsync();
            await _context.PurchaseAtExports
                .Where(w => order!.ImportedProducts!.Select(s => s.Id).Contains(w.ImportedProductId)).LoadAsync();
            await _context.Products.Where(w => order!.ImportedProducts!.Select(s => s.ProductId).Contains(w.Id))
                .Include(i => i.Brand).LoadAsync();
            return order ?? throw new InvalidOperationException();
        }

        public async Task<SortPagedResponse<ImportedOrder>> GetBySortPagedSearchChapterAsync(
            SortPagedRequest<FilterOrderTuple> request, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value)
                .FirstOrDefault();
            if (userId == null) return new SortPagedResponse<ImportedOrder>();
            var accessCollection = _context.AccessRestrictions;
            var accessedComponents = accessCollection.Select(s => s.AccessedComponentId);
            var data = _context.ImportedOrders.Where(w => w.OrderIsOver == request.FilterTuple!.IsHistoryOrders).Select(
                s => new ImportedOrder()
                {
                    Id = s.Id,
                    Number = s.Number,
                    OurCompany = s.OurCompany,
                    OurCompanyId = s.OurCompanyId,
                    SellersCompany = s.SellersCompany,
                    SellersCompanyId = s.SellersCompanyId,
                    EmployeeSeller = s.EmployeeSeller,
                    OurEmployee = s.OurEmployee,
                    EmployeeSellerId = s.EmployeeSellerId,
                    OurEmployeeId = s.OurEmployeeId,
                    DateTimeCreated = s.DateTimeCreated,
                    DateTimeUpdate = s.DateTimeUpdate,
                    ImportedProducts = s.ImportedProducts,
                    TransactionCurrency = s.TransactionCurrency,
                    SupplierCurrency = s.SupplierCurrency,
                    LocalCurrency = s.LocalCurrency,
                    CurrencyPercent = s.CurrencyPercent,
                    Nds = s.Nds,
                    OrderIsWarn = s.OrderIsWarn,
                    OrderIsOver = s.OrderIsOver,
                    OrderIsLock = s.OrderIsLock,
                    IsAllProductsAreCollected = s.IsAllProductsAreCollected,
                    IsDeleted = s.IsDeleted,
                    IsFullDeleted = s.IsFullDeleted,
                    ImportedOrderStatusImportedOrders = s.ImportedOrderStatusImportedOrders,
                    ImportedOrderStatus = s.ImportedOrderStatus,

                });
            data = data.Where(w => (
                (w.SellersCompanyId != null && (!accessedComponents.Contains((Guid)w.SellersCompanyId) || (
                    accessedComponents.Contains((Guid)w.SellersCompanyId)
                    && accessCollection.First(f => f.AccessedComponentId == w.SellersCompanyId).AccessUsersToComponent
                        .Contains(userId))))));
            if (!string.IsNullOrEmpty(request.GlobalFilterValue))
            {
                    switch (request.GlobalFilterType)
                    {
                        case GlobalFilterTypes.ImportedOrder:
                            data = data.Where(w =>
                                w.Number != null && w.Number.ToLower().Contains(request.GlobalFilterValue.ToLower()));
                            break;
                        case GlobalFilterTypes.Product:
                        {
                            var searchedOrdersId = await _context.ImportedProducts
                                .Where(w => w.Product!.PartNumber!.Contains(request.GlobalFilterValue))
                                .Select(s => s.ImportedOrderId).ToListAsync();
                            data = data.Where(w => searchedOrdersId.Contains(w.Id));
                            break;
                        }
                        case GlobalFilterTypes.Company:
                            data = data.Where(w =>
                                w.OurCompany != null &&
                                w.OurCompany.Name.ToLower().Contains(request.GlobalFilterValue.ToLower()) ||
                                w.SellersCompany != null && w.SellersCompany.Name.ToLower()
                                    .Contains(request.GlobalFilterValue.ToLower()));
                            break;
                    }
            }
            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.IsOrdersWithUnreadComments)
                {
                    data = data.Where(wm =>
                        (_context.CommentOrders.Where(w => w.OrderId == wm.Id)
                            .Select(s => s.DateTimeUpdate).Any() && _context.LogUsersVisitToOrderComments
                            .FirstOrDefault(f => f.OrderOwnerCommentsId == wm.Id
                                                 && f.UserId == userId) == null)
                        || (_context.LogUsersVisitToOrderComments
                            .FirstOrDefault(f => f.OrderOwnerCommentsId == wm.Id
                                                 && f.UserId == userId)!.DateTimeVisit < _context.CommentOrders
                            .Where(w => w.OrderId == wm.Id)
                            .Select(s => s.DateTimeUpdate).Max()));
                }

                if (request.FilterTuple.CurrentStatusId != null && request.FilterTuple.CurrentStatusId != Guid.Empty)
                {
                    data = data.Where(w => w.ImportedOrderStatusImportedOrders!
                        .OrderByDescending(o => o.DateTimeCreate!.Value)
                        .FirstOrDefault()!.ImportedOrderStatusId == request.FilterTuple.CurrentStatusId);
                }

                if (request.FilterTuple.CurrentPartnerCompanyId != null &&
                    request.FilterTuple.CurrentPartnerCompanyId != Guid.Empty)
                {
                    data = data.Where(o => o.SellersCompanyId == request.FilterTuple.CurrentPartnerCompanyId);
                    if (request.FilterTuple.CurrentPartnerEmployeeId != null &&
                        request.FilterTuple.CurrentPartnerEmployeeId != Guid.Empty)
                    {
                        data = data.Where(o => o.EmployeeSellerId == request.FilterTuple.CurrentPartnerEmployeeId);
                    }
                }

                if (request.FilterTuple.CurrentOurCompanyId != null &&
                    request.FilterTuple.CurrentOurCompanyId != Guid.Empty)
                {
                    data = data.Where(o => o.OurCompanyId == request.FilterTuple.CurrentOurCompanyId);
                    if (request.FilterTuple.CurrentOurEmployeeId != null &&
                        request.FilterTuple.CurrentOurEmployeeId != Guid.Empty)
                    {
                        data = data.Where(o => o.OurEmployeeId == request.FilterTuple.CurrentOurEmployeeId);
                    }
                }

                if (request.FilterTuple.CreateDateFirst != null)
                {
                    data = data.Where(w =>
                        w.DateTimeCreated!.Value.Date >= request.FilterTuple.CreateDateFirst.Value.Date);
                }

                if (request.FilterTuple.CreateDateLast != null)
                {
                    data = data.Where(w =>
                        w.DateTimeCreated!.Value.Date <= request.FilterTuple.CreateDateLast.Value.Date);
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
                if (request.FilterTuple.OurCompanies != null && request.FilterTuple.OurCompanies.Any())
                {
                    data = data.Where(o => request.FilterTuple.OurCompanies.Contains((Guid)o.OurCompanyId!));
                }

                if (request.FilterTuple.ContragentsCompanies != null && request.FilterTuple.ContragentsCompanies.Any())
                {
                    data = data.Where(o =>
                        request.FilterTuple.ContragentsCompanies.Contains((Guid)o.SellersCompanyId!));
                }
            }

            if (!string.IsNullOrEmpty(request.SearchString))
            {
                if (request.SearchInChapter != null)
                {
                    switch (request.SearchInChapter)
                    {
                        case SearchChapterNames.OrderNumber:
                            data = data.Where(w =>
                                w.Number != null && w.Number.ToLower().Contains(request.SearchString.ToLower()));
                            break;
                        case SearchChapterNames.ProductPartNumber:
                        {
                            var searchedOrdersId = await _context.ImportedProducts
                                .Where(w => w.Product!.PartNumber!.Contains(request.SearchString))
                                .Select(s => s.ImportedOrderId).ToListAsync();
                            data = data.Where(w => searchedOrdersId.Contains(w.Id));
                            break;
                        }
                        case SearchChapterNames.CompanyName:
                            data = data.Where(w =>
                                w.OurCompany != null &&
                                w.OurCompany.Name.ToLower().Contains(request.SearchString.ToLower()) ||
                                w.SellersCompany != null && w.SellersCompany.Name.ToLower()
                                    .Contains(request.SearchString.ToLower()));
                            break;
                    }
                }
            }

            var totalItems = data.Count();

            switch (request.SortLabel)
            {
                case "ourCompany_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.OurCompany);
                    break;
                case "conterCompany_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.SellersCompany);
                    break;
                case "name_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Number);
                    break;
                case "number_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Id);
                    break;
                case "create_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeCreated);
                    break;
                case "update_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeUpdate);
                    break;
            }

            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return new SortPagedResponse<ImportedOrder>()
                { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() };

        }

        public async Task<Guid> PostAsync(ImportedOrder importedOrder)
        {
            importedOrder.DateTimeCreated = DateTime.Now;
            importedOrder.DateTimeUpdate = DateTime.Now;
            var count = _context.ImportedOrders.Count(w =>
                w.DateTimeCreated!.Value.Date == importedOrder.DateTimeCreated!.Value.Date);
            importedOrder.Number = (importedOrder.DateTimeCreated!.Value.ToString("yyyyMMdd")) + (count + 1);
            _context.Entry(importedOrder).State = EntityState.Added;
            var status = _context.ImportedOrderStatus.FirstOrDefault(f => f.Position == 0);
            if (status != null)
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
            }

            await _context.SaveChangesAsync();
            return importedOrder.Id;
        }

        public async Task<Guid> PutAsync(ImportedOrder importedOrder)
        {
            _context.Entry(importedOrder).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return importedOrder.Id;
        }

        public async Task<int> PutRangeAsync(IEnumerable<ImportedOrder> importedOrders)
        {
            foreach (var importedOrder in importedOrders)
            {
                _context.Entry(importedOrder).State = EntityState.Modified;
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = await _context.ImportedOrders.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return 0;

            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<ImportedOrder> importedOrders)
        {
            _context.RemoveRange(importedOrders);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddStatusToOrderAsync(ImportedOrderStatusImportedOrder statusOrder, ClaimsPrincipal user)
        {
            var statusItem =
                await _context.ImportedOrderStatus.FirstOrDefaultAsync(f =>
                    f.Id == statusOrder.ImportedOrderStatusId);
            if (statusItem == null) return 0;
        
            var orderInDb = await _context.ImportedOrders
                .Include(i => i.ImportedOrderStatusImportedOrders).AsNoTracking()
                .Include(i => i.ImportedProducts!).ThenInclude(t => t.PurchaseAtStorageList)
                .Include(i => i.ImportedProducts!).ThenInclude(t => t.PurchaseAtExportList)!
                .ThenInclude(purchaseAtExport => purchaseAtExport.ExportedProduct)
                .Include(exportedOrder => exportedOrder.SellersCompany)
                .FirstOrDefaultAsync(x => x.Id == statusOrder.ImportedOrderId);

            if (orderInDb!.ImportedOrderStatusImportedOrders != null &&
                orderInDb.ImportedOrderStatusImportedOrders.Select(s => s.ImportedOrderStatusId)
                    .Contains(statusOrder.ImportedOrderStatusId))
            {
                //_context.Update(exportedOrderStatus);
                _context.Entry(statusOrder).State = EntityState.Modified;
            }
            else
                _context.Entry(statusOrder).State = EntityState.Added;

            if (statusItem is { Position: < 0 })
            {
                if (orderInDb.ImportedProducts != null && orderInDb.ImportedProducts.Count != 0)
                {
                    var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value)
                        .FirstOrDefault();
                    if (userId == null) return 0;
                    var ordersFromAddedComment = new List<Guid>();
                    foreach (var importedProduct in orderInDb.ImportedProducts)
                    {
                        if (importedProduct.PurchaseAtStorageList != null)
                            foreach (var sld in importedProduct.PurchaseAtStorageList)
                            {
                                sld.Quantity = 0;
                            }

                        if (importedProduct.PurchaseAtExportList != null)
                        {
                            foreach (var exp in importedProduct.PurchaseAtExportList)
                            {
                                var expProduct = _context.ExportedProducts.AsNoTracking().Include(i => i.ExportedOrder)
                                    .ThenInclude(t => t!.CompanyBuyer)
                                    .FirstOrDefault(f => f.Id == exp.ExportedProductId);
                                var expOrder = expProduct?.ExportedOrder;
                                if (expOrder != null)
                                {
                                    var orderId = expOrder.Id;
                                    if (!ordersFromAddedComment.Contains(orderId))
                                    {
                                        ordersFromAddedComment.Add(orderId);
                                        expOrder.OrderIsWarn = true;
                                        _context.Entry(expOrder).State = EntityState.Modified;
                                        await _orderCommentsService.SaveCommentAsync(new CommentOrder()
                                        {
                                            OrderId = orderId,
                                            Value =
                                                $"Был отмен связанный заказ на импорт №{orderInDb.Number}, {orderInDb.SellersCompany?.Name}, перепроверьте наличие продуктов",
                                            FromUserId = userId,
                                            DateTimeCreated = DateTime.Now,
                                            DateTimeUpdate = DateTime.Now,
                                            IsWarningComment = true,
                                            OrderType = typeof(ExportedOrder).ToString(),
                                        }, user);
                                    }
                                }

                                exp.Quantity = 0;
                            }
                        }

                        await _importedProductService.UpdateSourcesOrderItems(importedProduct);
                    }
                }
            }

            if (statusItem.Value == ImportOrderStatusNames.Completed)
            {
                orderInDb.OrderIsOver = true;
               // _context.Entry(orderInDb).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var exL =  new ImportedProduct[orderInDb.ImportedProducts!.Count];
                orderInDb.ImportedProducts.CopyTo(exL,0);
        
                foreach (var importedProduct in exL)
                {
                    if (importedProduct.PurchaseAtExportList == null) continue;
                    var ordersIsComplete = new List<Guid>();
                    foreach (var exp in importedProduct.PurchaseAtExportList)
                    {
                        var expProduct = _context.ExportedProducts.Include(i => i.ExportedOrder)
                            .ThenInclude(t => t!.CompanyBuyer)
                            .FirstOrDefault(f => f.Id == exp.ExportedProductId);
                        var expOrder = expProduct?.ExportedOrder;
                        if (expOrder is { IsAllProductsAreCollected: false } && !ordersIsComplete.Contains(expOrder.Id))
                        {
                            ordersIsComplete.Add(expOrder.Id);
                           var countNotOver = await _exportedOrderService.CheckOrderToOverImport(expOrder.Id);
                           if (countNotOver == 0)
                           {
                               expOrder.IsAllProductsAreCollected = true;
                               _context.Entry(expOrder).State = EntityState.Modified;
                           }
                        }
                        await _importedProductService.UpdateSourcesOrderItems(importedProduct, true);
                    }
                }
            }
            
            orderInDb.OrderIsLock = statusItem.Value != ImportOrderStatusNames.BeginFormed;
            _context.Entry(orderInDb).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
    }
}

