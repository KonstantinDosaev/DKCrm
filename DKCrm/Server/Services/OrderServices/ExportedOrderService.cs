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

        public ExportedOrderService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExportedOrder>> GetAsync()
        {
            return (await _context.ExportedOrders.Select(s => new ExportedOrder()
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
        public async Task<ExportedOrder> GetDetailAsync(Guid id)
        {
            var order = await _context.ExportedOrders.Select(s => new ExportedOrder()
            {
                Id = s.Id,
                Number = s.Number,
                OurCompany = s.OurCompany,
                CompanyBuyer = s.CompanyBuyer,
                EmployeeBuyer = s.EmployeeBuyer,
                OurEmployee = s.OurEmployee,
                EmployeeBuyerId = s.EmployeeBuyerId,
                OurEmployeeId = s.OurEmployeeId,
                ApplicationOrderingProducts = s.ApplicationOrderingProducts,
                ExportedProducts = s.ExportedProducts,
                TransactionCurrency = s.TransactionCurrency,
                BuyerCurrency = s.BuyerCurrency,
                LocalCurrency = s.LocalCurrency,
                CurrencyPercent = s.CurrencyPercent,
                Nds = s.Nds
            }).FirstOrDefaultAsync(a => a.Id == id);
            await _context.SoldFromStorages.Where(w => order!.ExportedProducts!.Select(s => s.Id).Contains(w.ExportedProductId)).LoadAsync();
            await _context.PurchaseAtExports.Where(w => order!.ExportedProducts!.Select(s => s.Id).Contains(w.ExportedProductId)).LoadAsync();
            await _context.Products.Where(w => order!.ExportedProducts!.Select(s => s.ProductId).Contains(w.Id)).Include(i => i.Brand).LoadAsync();
            return order ?? throw new InvalidOperationException();
            //var result = await _context.ExportedOrders
            //    .Include(i=>i.OurCompany).ThenInclude(i=>i!.Employees)
            //    .Include(i => i.CompanyBuyer).ThenInclude(i=>i!.Employees)
            //    .Include(i => i.ApplicationOrderingProducts).ThenInclude(t=>t!.ProductList)
            //    .Include(i => i.ExportedProducts)!.ThenInclude(t=>t.Product).ThenInclude(t=>t!.Brand)
            //    .AsSingleQuery().FirstOrDefaultAsync(a => a.Id == id);
            //return Ok(result);
        }
        public async Task<SortPagedResponse<ExportedOrder>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderTuple> request)
        {
            var data = _context.ExportedOrders.Where(w => w.OrderIsOver == request.FilterTuple!.IsHistoryOrders).Select(s => new ExportedOrder()
            {
                Id = s.Id,
                Number = s.Number,
                CurrencyPercent = s.CurrencyPercent,
                Nds = s.Nds,
                OurCompany = s.OurCompany,OurCompanyId = s.OurCompanyId,
                CompanyBuyer = s.CompanyBuyer,CompanyBuyerId = s.CompanyBuyerId,
                OurEmployee = s.OurEmployee, OurEmployeeId = s.OurEmployeeId,
                EmployeeBuyer = s.EmployeeBuyer, EmployeeBuyerId = s.EmployeeBuyerId,
                DateTimeCreated = s.DateTimeCreated,
                DateTimeUpdate = s.DateTimeUpdate,
                ExportedProducts = s.ExportedProducts,
                ExportedOrderStatus = s.ExportedOrderStatus,
                ExportedOrderStatusExported = s.ExportedOrderStatusExported,
               IsAllProductsAreCollected = s.IsAllProductsAreCollected
                
            });
            //if (request.Chapter != null && request.ChapterId != null)
            //{
            //    data = data.Where(o => o.ExportedOrderStatusId == request.ChapterId);
            //}

            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.IsCompleteOrders != null)
                {
                    data = data.Where(w => w.IsAllProductsAreCollected == request.FilterTuple.IsCompleteOrders);
                }
                if (request.FilterTuple.CurrentStatusId != null && request.FilterTuple.CurrentStatusId != Guid.Empty)
                {
                    data = data.Where(o => o.ExportedOrderStatusExported!.OrderBy(o => o.DateTimeCreate!.Value).LastOrDefault()!.ExportedOrderStatusId == request.FilterTuple.CurrentStatusId);
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
                if (request.FilterTuple.OurCompanies != null && request.FilterTuple.OurCompanies.Any())
                {
                    data = data.Where(o => request.FilterTuple.OurCompanies.Contains((Guid)o.OurCompanyId!));
                }
                if (request.FilterTuple.ContragentsCompanies != null && request.FilterTuple.ContragentsCompanies.Any())
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
                                var searchedOrdersId = await _context.ImportedProducts.Where(w => w.Product!.PartNumber!.Contains(request.SearchString))
                                    .Select(s => s.ImportedOrderId).ToListAsync();
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

            return new SortPagedResponse<ExportedOrder>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() };
        }
        public async Task<Guid> PostAsync(ExportedOrder exportedOrder, string userName)
        {

            exportedOrder.DateTimeCreated = DateTime.Now;
            var count = _context.ExportedOrders.Count(w => w.DateTimeCreated!.Value.Date == exportedOrder.DateTimeCreated!.Value.Date);
            exportedOrder.Number = (exportedOrder.DateTimeCreated!.Value.ToShortDateString()).Replace(".", "") + (count + 1);
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
        public async Task<Guid> PutAsync(ExportedOrder exportedOrder, string userName)
        {
            exportedOrder.DateTimeUpdate = DateTime.Now;
            exportedOrder.UpdatedUser = userName;
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
            _context.Remove(new ExportedOrder { Id = id });
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteRangeAsync(IEnumerable<ExportedOrder> exportedOrders)
        {
            _context.RemoveRange(exportedOrders);

            return await _context.SaveChangesAsync();
        }
        public async Task<int> AddStatusToOrderAsync(ExportedOrderStatusExportedOrder exportedOrderStatus)
        {
            _context.Entry(exportedOrderStatus).State = EntityState.Added;
            return await _context.SaveChangesAsync();
        }
        public async Task<int> RemoveStatusFromOrderAsync(ExportedOrderStatusExportedOrder exportedOrderStatus)
        {
            _context.Entry(exportedOrderStatus).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
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
