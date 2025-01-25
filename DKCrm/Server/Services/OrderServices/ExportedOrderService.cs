using System.Security.Claims;
using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DKCrm.Server.Services.OrderServices
{
    public class ExportedOrderService : IExportedOrderService
    {
        private readonly ApplicationDBContext _context;
        private readonly IExportedProductService _exportedProductService;
        private readonly IOrderCommentsService _orderCommentsService;

        public ExportedOrderService(ApplicationDBContext context, IExportedProductService exportedProductService, IOrderCommentsService orderCommentsService)
        {
            _context = context;
            _exportedProductService = exportedProductService;
            _orderCommentsService = orderCommentsService;
        }

        public async Task<IEnumerable<ExportedOrder>> GetAsync(ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null) return Array.Empty<ExportedOrder>();
            var accessCollection =  _context.AccessRestrictions;
            var accessedComponents =  accessCollection.Select(s => s.AccessedComponentId);

            return (await _context.ExportedOrders.Where(w=> !accessedComponents.Any() || (w.CompanyBuyerId!= null && (!accessedComponents.Contains((Guid)w.CompanyBuyerId) ||(
                   accessedComponents.Contains((Guid)w.CompanyBuyerId)
                   && accessCollection.First(f=>f.AccessedComponentId == w.CompanyBuyerId).AccessUsersToComponent.Contains(userId))))).Select(s => new ExportedOrder()
            {
                Id = s.Id,
                Number = s.Number,
                OurCompany = s.OurCompany,
                CompanyBuyer = s.CompanyBuyer,
                EmployeeBuyer = s.EmployeeBuyer,
                OurEmployee = s.OurEmployee,
                ExportedOrderStatus = s.ExportedOrderStatus,
                ExportedOrderStatusExported = s.ExportedOrderStatusExported,
            }).ToListAsync());
        }
        public async Task<ExportedOrder> GetDetailAsync(Guid id, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null) return new ExportedOrder();
            var accessCollection = _context.AccessRestrictions;
            var accessedComponents = accessCollection.Select(s => s.AccessedComponentId);
            var order = await _context.ExportedOrders.Where(w=> (
                (w.CompanyBuyerId != null && (!accessedComponents.Contains((Guid)w.CompanyBuyerId) || (
                    accessedComponents.Contains((Guid)w.CompanyBuyerId)
                    && accessCollection.First(f => f.AccessedComponentId == w.CompanyBuyerId).AccessUsersToComponent
                        .Contains(userId)))))).Select(s => new ExportedOrder()
            {
                Id = s.Id,
                Number = s.Number,
                OurCompany = s.OurCompany,
                OurCompanyId = s.OurCompanyId,
                CompanyBuyer = s.CompanyBuyer,
                CompanyBuyerId = s.CompanyBuyerId,
                EmployeeBuyer = s.EmployeeBuyer,
                OurEmployee = s.OurEmployee,
                EmployeeBuyerId = s.EmployeeBuyerId,
                OurEmployeeId = s.OurEmployeeId,
                
                IsAllProductsAreCollected = s.IsAllProductsAreCollected,
                DateTimeCreated = s.DateTimeCreated,
                DateTimeUpdate = s.DateTimeUpdate,
                OrderIsOver = s.OrderIsOver,
                IsDeleted = s.IsDeleted,
                IsFullDeleted = s.IsFullDeleted,
                OrderIsWarn = s.OrderIsWarn,
                ApplicationOrderingProducts = s.ApplicationOrderingProducts,
                ExportedProducts = s.ExportedProducts,
                TransactionCurrency = s.TransactionCurrency,
                BuyerCurrency = s.BuyerCurrency,
                LocalCurrency = s.LocalCurrency,
                CurrencyPercent = s.CurrencyPercent,
                Nds = s.Nds,
                OrderIsLock = s.OrderIsLock,
            }).FirstOrDefaultAsync(a => a.Id == id);
            await _context.SoldFromStorages.Where(w => order!.ExportedProducts!.Select(s => s.Id).Contains(w.ExportedProductId)).LoadAsync();
            await _context.PurchaseAtExports.Where(w => order!.ExportedProducts!.Select(s => s.Id).Contains(w.ExportedProductId)).LoadAsync();
            await _context.Products.Where(w => order!.ExportedProducts!.Select(s => s.ProductId).Contains(w.Id)).Include(i => i.Brand).LoadAsync();
            return order ?? throw new InvalidOperationException();
        }
        public async Task<SortPagedResponse<ExportedOrder>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderTuple> request, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null) return new SortPagedResponse<ExportedOrder>();
            var accessCollection =  _context.AccessRestrictions;
            var accessedComponents = accessCollection.Select(s => s.AccessedComponentId);
            var data = _context.ExportedOrders.Where(w =>( w.OrderIsOver == request.FilterTuple!.IsHistoryOrders) ).Select(s => new ExportedOrder()
            {
                Id = s.Id,
                Number = s.Number,
                CurrencyPercent = s.CurrencyPercent,
                Nds = s.Nds,
                BuyerCurrency = s.BuyerCurrency,
                LocalCurrency = s.LocalCurrency,
                IsAllProductsAreCollected = s.IsAllProductsAreCollected,
                DateTimeCreated = s.DateTimeCreated,
                DateTimeUpdate = s.DateTimeUpdate,
                TransactionCurrency = s.TransactionCurrency, 
                ExportedProducts = s.ExportedProducts,
                OrderIsOver = s.OrderIsOver,
                OurCompanyId = s.OurCompanyId, 
                IsDeleted = s.IsDeleted,
                IsFullDeleted = s.IsFullDeleted,
                CompanyBuyerId = s.CompanyBuyerId,
                OrderIsLock = s.OrderIsLock,
                OrderIsWarn = s.OrderIsWarn,
                
                ExportedOrderStatus = s.ExportedOrderStatus,
                ExportedOrderStatusExported = s.ExportedOrderStatusExported,
               OurCompany = s.OurCompany,
               CompanyBuyer = s.CompanyBuyer,
               OurEmployee = s.OurEmployee, OurEmployeeId = s.OurEmployeeId,
               EmployeeBuyer = s.EmployeeBuyer, EmployeeBuyerId = s.EmployeeBuyerId,
               ApplicationOrderingProducts = s.ApplicationOrderingProducts,
                
            });
            data = data.Where(w => (
                (w.CompanyBuyerId != null && (!accessedComponents.Contains((Guid)w.CompanyBuyerId) || (
                    accessedComponents.Contains((Guid)w.CompanyBuyerId)
                    && accessCollection.First(f => f.AccessedComponentId == w.CompanyBuyerId).AccessUsersToComponent
                        .Contains(userId))))));
           
            if (!string.IsNullOrEmpty(request.GlobalFilterValue))
            {
                    switch (request.GlobalFilterType)
                    {
                        case GlobalFilterTypes.ExportedOrder:
                            data = data.Where(w =>
                                w.Number != null && w.Number.ToLower().Contains(request.GlobalFilterValue.ToLower()));
                            break;
                        case GlobalFilterTypes.Product:
                        {
                            var searchedOrdersId = await _context.ExportedProducts
                                .Where(w => w.Product!.PartNumber!.Contains(request.GlobalFilterValue))
                                .Select(s => s.ExportedOrderId).ToListAsync();
                            data = data.Where(w => searchedOrdersId.Contains(w.Id));
                            break;
                        }
                        case GlobalFilterTypes.Company:
                            data = data.Where(w =>
                                w.OurCompany != null && w.OurCompany.Name.ToLower().Contains(request.GlobalFilterValue.ToLower()) ||
                                w.CompanyBuyer != null && w.CompanyBuyer.Name.ToLower().Contains(request.GlobalFilterValue.ToLower()));
                            break;
                    }
            }
            
            var unreadAny = data.Any(wm =>
                (_context.CommentOrders.Where(w => w.OrderId == wm.Id)
                    .Select(s => s.DateTimeUpdate).Any() && _context.LogUsersVisitToOrderComments
                    .FirstOrDefault(f => f.OrderOwnerCommentsId == wm.Id
                                         && f.UserId == userId) == null)
                || (_context.LogUsersVisitToOrderComments
                    .FirstOrDefault(f => f.OrderOwnerCommentsId == wm.Id
                                         && f.UserId == userId)!.DateTimeVisit < _context.CommentOrders
                    .Where(w => w.OrderId == wm.Id)
                    .Select(s => s.DateTimeUpdate).Max()));
            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.IsOrdersWithUnreadComments)
                {
                    data = data.Where(wm =>
                        ( _context.CommentOrders.Where(w=>w.OrderId == wm.Id)
                            .Select(s=>s.DateTimeUpdate).Any() && _context.LogUsersVisitToOrderComments
                            .FirstOrDefault(f => f.OrderOwnerCommentsId == wm.Id 
                                                 && f.UserId == userId) == null)
                    ||(_context.LogUsersVisitToOrderComments
                        .FirstOrDefault(f=>f.OrderOwnerCommentsId == wm.Id 
                                           && f.UserId == userId)!.DateTimeVisit < _context.CommentOrders
                            .Where(w => w.OrderId == wm.Id)
                            .Select(s => s.DateTimeUpdate).Max()) );
                }
                if (request.FilterTuple.IsCompleteOrders != null)
                {
                    data = data.Where(w => w.IsAllProductsAreCollected == request.FilterTuple.IsCompleteOrders);
                }
                if (request.FilterTuple.CurrentStatusId != null && request.FilterTuple.CurrentStatusId != Guid.Empty)
                {
                    data = data.Where(w => w.ExportedOrderStatusExported!.OrderByDescending(o => o.DateTimeCreate!.Value).FirstOrDefault()!.ExportedOrderStatusId == request.FilterTuple.CurrentStatusId);
                }
                if (request.FilterTuple.CurrentPartnerCompanyId != null && request.FilterTuple.CurrentPartnerCompanyId != Guid.Empty)
                {
                    data = data.Where(o => o.CompanyBuyerId == request.FilterTuple.CurrentPartnerCompanyId);
                    if (request.FilterTuple.CurrentPartnerEmployeeId != null && request.FilterTuple.CurrentPartnerEmployeeId != Guid.Empty)
                    {
                        data = data.Where(o => o.EmployeeBuyerId == request.FilterTuple.CurrentPartnerEmployeeId);
                    }
                }
                if (request.FilterTuple.CurrentOurCompanyId != null && request.FilterTuple.CurrentOurCompanyId != Guid.Empty)
                {
                    data = data.Where(o => o.OurCompanyId == request.FilterTuple.CurrentOurCompanyId);
                    if (request.FilterTuple.CurrentOurEmployeeId != null && request.FilterTuple.CurrentOurEmployeeId != Guid.Empty)
                    {
                        data = data.Where(o => o.OurEmployeeId == request.FilterTuple.CurrentOurEmployeeId);
                    }
                }
                if (request.FilterTuple.CreateDateFirst != null)
                {
                    data = data.Where(w => w.DateTimeCreated!.Value.Date >= request.FilterTuple.CreateDateFirst.Value.Date);
                }
                if (request.FilterTuple.CreateDateLast != null)
                {
                    data = data.Where(w => w.DateTimeCreated!.Value.Date <= request.FilterTuple.CreateDateLast.Value.Date);
                }
                if (request.FilterTuple.UpdateDateFirst != null && request.FilterTuple.UpdateDateLast != null)
                {
                    data = data.Where(w => w.DateTimeUpdate!.Value.Date >= request.FilterTuple.UpdateDateFirst.Value.Date
                                           && w.DateTimeUpdate.Value.Date <= request.FilterTuple.UpdateDateLast.Value.Date);
                }
                if (request.FilterTuple.OurCompanies != null && request.FilterTuple.OurCompanies.Count != 0)
                {
                    data = data.Where(o => request.FilterTuple.OurCompanies.Contains((Guid)o.OurCompanyId!));
                }
                if (request.FilterTuple.ContragentsCompanies != null && request.FilterTuple.ContragentsCompanies.Count != 0)
                {
                    data = data.Where(o => request.FilterTuple.ContragentsCompanies.Contains((Guid)o.CompanyBuyerId!));
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
                                var searchedOrdersId = await _context.ExportedProducts
                                    .Where(w => w.Product!.PartNumber!.Contains(request.SearchString))
                                    .Select(s => s.ExportedOrderId).ToListAsync();
                                data = data.Where(w => searchedOrdersId.Contains(w.Id));
                                break;
                            }
                        case SearchChapterNames.CompanyName:
                            data = data.Where(w =>
                                w.OurCompany != null && w.OurCompany.Name.ToLower().Contains(request.SearchString.ToLower()) ||
                                w.CompanyBuyer != null && w.CompanyBuyer.Name.ToLower().Contains(request.SearchString.ToLower()));
                            break;
                    }
                }
            }
         
            var totalItems = data.Any() ? data.Count() : 0;
            if (request.SortLabel != null)
            {
                switch (request.SortLabel)
                {
                    case "ourCompany_field":
                        data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.OurCompany);
                        break;
                    case "conterCompany_field":
                        data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.CompanyBuyer);
                        break;
                    case "number_field":
                        data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Number);
                        break;
                    case "create_field":
                        data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeCreated);
                        break;
                    case "update_field":
                        data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeUpdate);
                        break;
                   
                }
            }

            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return new SortPagedResponse<ExportedOrder>()
            {
                TotalItems = totalItems, 
                Items = await data.AsSingleQuery().ToListAsync(),
                AnyUnreadComment = unreadAny
            };
        }
        public async Task<Guid> PostAsync(ExportedOrder exportedOrder, string userName)
        {

            exportedOrder.DateTimeCreated = DateTime.Now;
            var count = _context.ExportedOrders.Count(w => w.DateTimeCreated!.Value.Date == exportedOrder.DateTimeCreated!.Value.Date);
            exportedOrder.Number = (exportedOrder.DateTimeCreated!.Value.ToString("ddMMyyyy")) + (count + 1);
            _context.Entry(exportedOrder).State = EntityState.Added;

            //if (exportedOrder.ExportedOrderStatus != null)
            //    _context.Entry(exportedOrder.ExportedOrderStatus).State = EntityState.Modified;

            var status = _context.ExportedOrderStatus.FirstOrDefault(f => f.Position == 0);
            if (status != null)
            {
                exportedOrder.ExportedOrderStatus ??= new List<ExportedOrderStatus>();
                exportedOrder.ExportedOrderStatus.Add(status);
            }

            if (exportedOrder.ApplicationOrderingProducts != null)
            {
                exportedOrder.ApplicationOrderingProducts.DateTimeUpdate = DateTime.Now;
                exportedOrder.ApplicationOrderingProducts.UpdatedUser = userName;
                exportedOrder.ApplicationOrderingProducts.TakerUser = userName;
                _context.Entry(exportedOrder.ApplicationOrderingProducts).State = EntityState.Modified;
            }

            if (exportedOrder.ExportedProducts != null)
            {
                foreach (var item in exportedOrder.ExportedProducts)
                {
                    _context.Entry(item).State = item.Id == Guid.Empty ? EntityState.Added : EntityState.Modified;
                }
            }
            await _context.SaveChangesAsync();
            return exportedOrder.Id;
        }
        public async Task<Guid> PutAsync(ExportedOrder exportedOrder, ClaimsPrincipal user)
        {
            exportedOrder.DateTimeUpdate = DateTime.Now;
            exportedOrder.UpdatedUser = user.Identity?.Name!;
            _context.Entry(exportedOrder).State = EntityState.Modified;

            if (exportedOrder.ApplicationOrderingProducts != null)
            {
                _context.Entry(exportedOrder.ApplicationOrderingProducts).State = EntityState.Modified;
            }
            if (exportedOrder.ExportedOrderStatus != null)
            {
                foreach (var status in exportedOrder.ExportedOrderStatus)
                {
                    _context.Entry(status).State = EntityState.Modified;
                }
            }
            if (exportedOrder.ExportedOrderStatusExported != null)
            {
                exportedOrder.ExportedOrderStatusExported = null;
            }
            if (exportedOrder.ExportedProducts != null)
            {
                foreach (var item in exportedOrder.ExportedProducts)
                {
                    _context.Entry(item).State = item.Id == Guid.Empty ? EntityState.Added : EntityState.Modified;
                }
            }
            await _context.SaveChangesAsync();
            return exportedOrder.Id;
        }
        public async Task<int> PutRangeAsync(IEnumerable<ExportedOrder> exportedOrders)
        {
            foreach (var order in exportedOrders)
            {
                _context.Entry(order).State = EntityState.Modified;
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            var orderInDb = await _context.ExportedOrders.Include(i=>i.ExportedProducts)
                .FirstOrDefaultAsync(x => x.Id == id);
            await _context.SoldFromStorages.Where(w => orderInDb!.ExportedProducts!
                .Select(s => s.Id).Contains(w.ExportedProductId)).LoadAsync();
            await _context.PurchaseAtExports.Where(w => orderInDb!.ExportedProducts!
                .Select(s => s.Id).Contains(w.ExportedProductId)).LoadAsync();

            if (orderInDb == null) return 0;
            if (orderInDb.ExportedProducts != null)
            {
                foreach (var exportedProduct in orderInDb.ExportedProducts)
                {
                    if (exportedProduct.SoldFromStorage != null)
                    {
                        var unused = exportedProduct.SoldFromStorage.Select(s => s.Quantity = 0);
                    }
                    if (exportedProduct.PurchaseAtExports != null)
                    {
                        var unused = exportedProduct.PurchaseAtExports.Select(s => s.Quantity = 0);
                    } 
                    await _exportedProductService.UpdateSourcesOrderItems(exportedProduct);
                }
            }
            _context.Entry(orderInDb).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteRangeAsync(IEnumerable<ExportedOrder> exportedOrders)
        {
            var countResult = 0;
            foreach (var order in exportedOrders)
            {
               countResult = await DeleteAsync(order.Id);
            }
            return countResult;
        }
        public async Task<int> AddStatusToOrderAsync(ExportedOrderStatusExportedOrder exportedOrderStatus, ClaimsPrincipal user)
        {
            var statusItem =
                await _context.ExportedOrderStatus.FirstOrDefaultAsync(f =>
                    f.Id == exportedOrderStatus.ExportedOrderStatusId);
            var orderInDb = await _context.ExportedOrders
                .Include(i => i.ExportedOrderStatusExported).AsNoTracking()
                .Include(i => i.ExportedProducts!).ThenInclude(t => t.SoldFromStorage)
                .Include(i => i.ExportedProducts!).ThenInclude(t => t.PurchaseAtExports)
                .Include(exportedOrder => exportedOrder.CompanyBuyer)
                .FirstOrDefaultAsync(x => x.Id == exportedOrderStatus.ExportedOrderId);

            if (orderInDb!.ExportedOrderStatusExported != null && 
                orderInDb.ExportedOrderStatusExported.Select(s=>s.ExportedOrderStatusId)
                    .Contains(exportedOrderStatus.ExportedOrderStatusId))
            {
               //_context.Update(exportedOrderStatus);
                _context.Entry(exportedOrderStatus).State = EntityState.Modified;
            }
            else
                _context.Entry(exportedOrderStatus).State = EntityState.Added;
            
            if (statusItem is { Position: < 0 })
            {
                //await _context.SoldFromStorages.Where(w => orderInDb!.ExportedProducts!
                //    .Select(s => s.Id).Contains(w.ExportedProductId)).LoadAsync();
                //await _context.PurchaseAtExports.Where(w => orderInDb!.ExportedProducts!
                //    .Select(s => s.Id).Contains(w.ExportedProductId)).LoadAsync();
                //if (orderInDb == null) return 0;
                if (orderInDb.ExportedProducts != null && orderInDb.ExportedProducts.Count != 0)
                {
                    var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
                    if (userId == null) return 0;
                    var ordersFromAddedComment = new List<Guid>();
                    foreach (var exportedProduct in orderInDb.ExportedProducts)
                    {
                        if (exportedProduct.SoldFromStorage != null)
                            foreach (var sld in exportedProduct.SoldFromStorage)
                            {
                                sld.Quantity = 0;
                            }

                        if (exportedProduct.PurchaseAtExports != null)
                        {
                            foreach (var exp in exportedProduct.PurchaseAtExports)
                            {
                                var impProduct = _context.ImportedProducts.
                                    Include(i=> i.ImportedOrder)
                                    .ThenInclude(t=>t!.SellersCompany).FirstOrDefault(f=> f.Id == exp.ImportedProductId);
                                var impOrder = impProduct?.ImportedOrder;
                                if (impOrder != null)
                                {
                                    var orderId = impOrder.Id;
                                    if (!ordersFromAddedComment.Contains(orderId))
                                    {
                                        ordersFromAddedComment.Add(orderId);
                                        impOrder.OrderIsWarn = true;
                                        _context.Entry(impOrder).State = EntityState.Modified;
                                        await _orderCommentsService.SaveCommentAsync(new CommentOrder()
                                        {
                                            OrderId = orderId,
                                            Value = $"Был отмен связанный заказ на экспорт №{orderInDb.Number}, {orderInDb.CompanyBuyer?.Name}, перепроверьте наличие продуктов",
                                            FromUserId = userId,
                                            DateTimeCreated = DateTime.Now,
                                            DateTimeUpdate = DateTime.Now,
                                            IsWarningComment = true,
                                            OrderType = typeof(ImportedOrder).ToString(),
                                        }, user);
                                    }
                                }

                                exp.Quantity = 0;
                            }
                        }
                        await _exportedProductService.UpdateSourcesOrderItems(exportedProduct);
                    }
                }
            }
            

            if (statusItem!.Value == ExportOrderStatusNames.Completed)
                orderInDb.OrderIsOver = true;
            
            orderInDb.OrderIsLock = statusItem.Value != ExportOrderStatusNames.BeginFormed;
            _context.Entry(orderInDb).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        public async Task<int> RemoveStatusFromOrderAsync(ExportedOrderStatusExportedOrder exportedOrderStatus)
        {
            _context.Entry(exportedOrderStatus).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CheckOrderToOverImport(Guid id)
        {
            var result = await _context.ExportedOrders.Where(f => f.Id == id)
                .Include(i => i.ExportedProducts)!.ThenInclude(t => t.ImportedProducts)!
                .ThenInclude(t => t.ImportedOrder).FirstOrDefaultAsync();
            var notOverImportOrder = 0;
            if (result!.ExportedProducts != null)
                foreach (var exportedProduct in result.ExportedProducts)
                {
                    if (exportedProduct.ImportedProducts != null)
                        foreach (var importedProduct in exportedProduct.ImportedProducts)
                        {
                            if (importedProduct.ImportedOrder!.OrderIsOver == false)
                            {
                                notOverImportOrder++;
                            }
                        }
                }

            return notOverImportOrder;
        }
        //public async Task<IActionResult> GetOrder(Guid id)
        //{
        //    //var dev = await _context.ExportedOrders.Select(s => new
        //    //{
        //    //    s.Id,
        //    //    s.Name,
        //    //    s.OurCompany,
        //    //    s.CompanyBuyer,
        //    //    s.EmployeeBuyer,
        //    //    s.OurEmployee,
        //    //    s.EmployeeBuyerId,
        //    //    s.OurEmployeeId,
        //    //    s.ApplicationOrderingProducts,
        //    //    s.ExportedProducts,
        //    //}).FirstOrDefaultAsync(a => a.Id == id);
        //    //return Ok(dev);
        //    var result = await _context.ExportedOrders
        //        .Include(i => i.OurCompany).ThenInclude(i => i!.Employees)
        //        .Include(i => i.CompanyBuyer).ThenInclude(i => i!.Employees)
        //        .Include(i => i.ApplicationOrderingProducts).ThenInclude(t => t!.ProductList)
        //        .Include(i => i.ExportedProducts)!.ThenInclude(t => t.Product).ThenInclude(t => t!.Brand)
        //        .AsSingleQuery().FirstOrDefaultAsync(a => a.Id == id);
        //    return Ok(result);
        //}
    }
}
