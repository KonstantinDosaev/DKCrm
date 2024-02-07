using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApplicationOrderingController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ApplicationOrderingController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
           
            return Ok(await _context.ApplicationOrderingProducts
                //.Include(i=>i.OurCompany).ThenInclude(t=>t!.Employees)
                //.Include(i=>i.SellersCompany).ThenInclude(t=>t!.Employees)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {

            var dev = await _context.ApplicationOrderingProducts
                .Include(i => i.ProductList!).ThenInclude(t => t.Brand).IgnoreQueryFilters()
                .Include(f => f.ApplicationOrderingProductProduct)
                .AsSingleQuery()
                .FirstOrDefaultAsync(a => a.Id == id);



            return Ok(dev);
        }
        [HttpPost]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterApplicationOrderTuple> request)
        {
            var data = _context.ApplicationOrderingProducts.Select(s => new ApplicationOrderingProducts()
            {
                Id = s.Id,
                Name = s.Name,
                CompanyName = s.CompanyName,
                CompanyInn = s.CompanyInn,
                ProductList = s.ProductList,
                UserName = s.UserName,
                DateTimeCreated = s.DateTimeCreated,
                DateTimeUpdate = s.DateTimeUpdate,
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
                    w.Name.Contains(request.SearchString)
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
                    data = data.Where(w => w.DateTimeUpdate!.Value.Date >= request.FilterTuple.UpdateDateFirst.Value.Date
                                           && w.DateTimeUpdate.Value.Date <= request.FilterTuple.UpdateDateLast.Value.Date);
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
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeUpdate);
                    break;
            }

            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return Ok(new SortPagedResponse<ApplicationOrderingProducts>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() });

        }

        [HttpPost]
        public async Task<IActionResult> Post(ApplicationOrderingProducts order)
        {

            // _context.ImportedOrders.Add(importedOrder);
            _context.Entry(order).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return Ok(order.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ApplicationOrderingProducts order)
        {
    
            _context.Entry(order).State = EntityState.Modified;
            if (order.ApplicationOrderingProductProduct != null)
            {
                var oldOrder = _context.ApplicationOrderingProducts
                    .Include(i => i.ProductList).AsNoTracking()
                    .FirstOrDefault(f => f.Id == order.Id);
                var productIds = oldOrder!.ProductList!
                    .Select(s=>s.Id).ToList();
                foreach (var item in order.ApplicationOrderingProductProduct)
                {
                    _context.Entry(item).State = productIds.Contains(item.ProductId) ? EntityState.Modified : EntityState.Added;
                }
            }
            await _context.SaveChangesAsync();
            return Ok(order);
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ApplicationOrderingProducts> order)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.ApplicationOrderingProducts.UpdateRange(order);
            await _context.SaveChangesAsync();
            return Ok(order.Count());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = await _context.ApplicationOrderingProducts.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return NoContent();

            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return Ok(dev);
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ApplicationOrderingProducts> orders)
        {
            _context.RemoveRange(orders);
            await _context.SaveChangesAsync();
            return Ok(orders.Count());
        }
    }
}
