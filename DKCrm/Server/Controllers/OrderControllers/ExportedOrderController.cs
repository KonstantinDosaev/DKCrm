using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExportedOrderController:ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ExportedOrderController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.ExportedOrders.Select(s=>new
            {
               s.Id, s.Name,s.OurCompany,s.CompanyBuyer,s.EmployeeBuyer,s.OurEmployee
            }).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            //var dev = await _context.ExportedOrders.Select(s => new
            //{
            //    s.Id,
            //    s.Name,
            //    s.OurCompany,
            //    s.CompanyBuyer,
            //    s.EmployeeBuyer,
            //    s.OurEmployee,
            //    s.EmployeeBuyerId,
            //    s.OurEmployeeId,
            //    s.ApplicationOrderingProducts,
            //    s.ExportedProducts,
            //}).FirstOrDefaultAsync(a => a.Id == id);
            //return Ok(dev);
            var result = await _context.ExportedOrders
                .Include(i=>i.OurCompany).ThenInclude(i=>i!.Employees)
                .Include(i => i.CompanyBuyer).ThenInclude(i=>i!.Employees)
                .Include(i => i.ApplicationOrderingProducts).ThenInclude(t=>t.ProductList)
                .Include(i => i.ExportedProducts)!.ThenInclude(t=>t.Product).ThenInclude(t=>t!.Brand)
                .AsSingleQuery().FirstOrDefaultAsync(a => a.Id == id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderTuple> request)
        {
            var data = _context.ExportedOrders.Select(s => new ExportedOrder()
            {
                Id = s.Id,
                Name = s.Name,
                ExportedProducts = s.ExportedProducts,
                OurCompany = s.OurCompany,
                CompanyBuyer = s.CompanyBuyer,
                OurEmployee = s.OurEmployee,
                EmployeeBuyer = s.EmployeeBuyer,
                ExportedOrderStatus = s.ExportedOrderStatus,
            }).Select(s => s);
            if (request.Chapter != null && request.ChapterId != null)
            {
                data = data.Where(o => o.ExportedOrderStatusId == request.ChapterId);
            }

            if (request.FilterTuple != null)
            {
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

            var totalItems = data.Count();

            switch (request.SortLabel)
            {
                case "ourCompany_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.OurCompany);
                    break;
                case "conterCompany_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.CompanyBuyer);
                    break;
                case "name_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Name);
                    break;
                case "number_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Id);
                    break;
            }
            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return Ok(new SortPagedResponse<ExportedOrder>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() });

        }

        [HttpPost]
        public async Task<IActionResult> Post(ExportedOrder exportedOrder)
        {
            exportedOrder.DateTimeCreated = DateTime.Now;
            var count = _context.ExportedOrders.Count(w => w.DateTimeCreated!.Value.Date == exportedOrder.DateTimeCreated!.Value.Date);
            exportedOrder.Name = (exportedOrder.DateTimeCreated!.Value.ToShortDateString()).Replace(".", "") + (count + 1);
            _context.Entry(exportedOrder).State = EntityState.Added;
            if (exportedOrder.ApplicationOrderingProducts != null)
            {
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
            return Ok(exportedOrder.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ExportedOrder exportedOrder)
        {
            exportedOrder.DateTimeUpdated = DateTime.Now;
            _context.Entry(exportedOrder).State = EntityState.Modified;

            if (exportedOrder.ApplicationOrderingProducts != null)
            {
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
            return Ok(exportedOrder);
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ExportedOrder> exportedOrders)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.ExportedOrders.UpdateRange(exportedOrders);
            await _context.SaveChangesAsync();
            return Ok(exportedOrders.Count());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = new ExportedOrder { Id = id };
            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ExportedOrder> exportedOrders)
        {
            _context.RemoveRange(exportedOrders);
            await _context.SaveChangesAsync();
            return Ok(exportedOrders.Count());
        }
    }
}
