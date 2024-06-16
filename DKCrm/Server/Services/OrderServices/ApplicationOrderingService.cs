using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DKCrm.Server.Services.OrderServices
{
    public class ApplicationOrderingService : IApplicationOrderingService
    {
        private readonly ApplicationDBContext _context;

        public ApplicationOrderingService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationOrderingProducts>> GetAsync()
        {

            return await _context.ApplicationOrderingProducts
                //.Include(i=>i.OurCompany).ThenInclude(t=>t!.Employees)
                //.Include(i=>i.SellersCompany).ThenInclude(t=>t!.Employees)
                .ToListAsync();
        }

        public async Task<ApplicationOrderingProducts> GetDetailAsync(Guid id)
        {

            var dev = await _context.ApplicationOrderingProducts
                .Include(i => i.ProductList!).ThenInclude(t => t.Brand).IgnoreQueryFilters()
                .Include(f => f.ApplicationOrderingProductProduct)
                .AsSingleQuery()
                .FirstOrDefaultAsync(a => a.Id == id);
            return dev ?? throw new InvalidOperationException();
        }
        public async Task<SortPagedResponse<ApplicationOrderingProducts>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterApplicationOrderTuple> request)
        {
            var data = _context.ApplicationOrderingProducts.Select(s => new ApplicationOrderingProducts()
            {
                Id = s.Id,
                Number = s.Number,
                CompanyName = s.CompanyName,
                CompanyInn = s.CompanyInn,
                ProductList = s.ProductList,
                UserName = s.UserName,
                DateTimeCreated = s.DateTimeCreated,
                DateTimeTake = s.DateTimeTake,
                ApplicationOrderingProductProduct = s.ApplicationOrderingProductProduct,
                InWork = s.InWork,
                IsDeleted = s.IsDeleted,
                Email = s.Email,
                UpdatedUser = s.UpdatedUser,
                MissingProductsInCatalog = s.MissingProductsInCatalog,
                Phone = s.Phone,
                UserId = s.UserId
            }).Select(s => s);
            if (request.FilterTuple is { UserId: { } })
            {
                data = data.Where(w => w.UserId == request.FilterTuple.UserId);
            }
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                data = data.Where(w =>
                    w.Number.Contains(request.SearchString)
                    || w.CompanyName!.ToLower().Contains(request.SearchString.ToLower()));
                if (request.FilterTuple != null)
                {
                    request.FilterTuple.CreateDateFirst = null;
                    request.FilterTuple.CreateDateLast = null;
                    request.FilterTuple.UpdateDateFirst = null;
                    request.FilterTuple.UpdateDateLast = null;
                }
            }
            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.InWork != null)
                {
                    data = request.FilterTuple.InWork == true ? data.Where(w => w.InWork == true) : data.Where(w => w.InWork == false);
                }

                if (request.FilterTuple.CreateDateFirst != null && request.FilterTuple.CreateDateLast != null)
                {
                    data = data.Where(w => w.DateTimeCreated!.Value.Date >= request.FilterTuple.CreateDateFirst.Value.Date
                                           && w.DateTimeCreated.Value.Date <= request.FilterTuple.CreateDateLast.Value.Date);
                }
                if (request.FilterTuple.UpdateDateFirst != null && request.FilterTuple.UpdateDateLast != null)
                {
                    data = data.Where(w => w.DateTimeTake!.Value.Date >= request.FilterTuple.UpdateDateFirst.Value.Date
                                           && w.DateTimeTake.Value.Date <= request.FilterTuple.UpdateDateLast.Value.Date);
                }
            }

            var totalItems = data.Count();
            var sd = request.SortDirection;
            switch (request.SortLabel)
            {
                case "create_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeCreated);
                    break;
                case "update_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeTake);
                    break;
            }

            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return new SortPagedResponse<ApplicationOrderingProducts>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() };

        }

        public async Task<Guid> PostAsync(ApplicationOrderingProducts order)
        {
            order.DateTimeCreated = DateTime.Now;
            // _context.ImportedOrders.Add(importedOrder);
            _context.Entry(order).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<Guid> PutAsync(ApplicationOrderingProducts order)
        {
            order.DateTimeUpdate = DateTime.Now;
            _context.Entry(order).State = EntityState.Modified;
            if (order.ApplicationOrderingProductProduct != null)
            {
                var oldOrder = _context.ApplicationOrderingProducts
                    .Include(i => i.ProductList).AsNoTracking()
                    .FirstOrDefault(f => f.Id == order.Id);
                var productIds = oldOrder!.ProductList!
                    .Select(s => s.Id).ToList();
                foreach (var item in order.ApplicationOrderingProductProduct)
                {
                    _context.Entry(item).State = productIds.Contains(item.ProductId) ? EntityState.Modified : EntityState.Added;
                }
            }
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<int> PutRangeAsync(IEnumerable<ApplicationOrderingProducts> order)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.ApplicationOrderingProducts.UpdateRange(order);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = await _context.ApplicationOrderingProducts.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return 0;
            _context.Remove(dev);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<ApplicationOrderingProducts> orders)
        {
            _context.RemoveRange(orders);
            return await _context.SaveChangesAsync();
        }
    }
}
