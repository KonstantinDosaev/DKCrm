using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImportedOrderController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ImportedOrderController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _context.Companies
                .Where(w => w.ImportedOrdersOurCompany!.Any() || w.ImportedOrdersSellersCompany!.Any())
                .Include(i=>i.Employees).LoadAsync();


            return Ok(await _context.ImportedOrders
                //.Include(i=>i.OurCompany).ThenInclude(t=>t!.Employees)
                //.Include(i=>i.SellersCompany).ThenInclude(t=>t!.Employees)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {

            var dev = await _context.ImportedOrders
                .Include(i=>i.ImportedProducts!).ThenInclude(t=>t.Product).ThenInclude(t=>t!.Brand).IgnoreQueryFilters()
                .Include(f => f.OurCompany)
                .Include(i=>i.SellersCompany)
                .Include(i => i.OurEmployee)
                .Include(i => i.EmployeeSeller).AsSingleQuery()
                .FirstOrDefaultAsync(a => a.Id == id);



            return Ok(dev);
        }
        [HttpPost]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderTuple> request)
        {
            var data = _context.ImportedOrders.Select(s => new ImportedOrder()
            {
                Id = s.Id,
                Name = s.Name,
                ImportedProducts = s.ImportedProducts,
                OurCompany = s.OurCompany,
                SellersCompany = s.SellersCompany,
                OurEmployee = s.OurEmployee,
                EmployeeSeller = s.EmployeeSeller,
                ImportedOrderStatusId = s.ImportedOrderStatusId,
            }).Select(s => s);
            if (request.Chapter != null && request.ChapterId != null)
            {
                data = data.Where(o => o.ImportedOrderStatusId == request.ChapterId);
            }

            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.OurCompanies != null && request.FilterTuple.OurCompanies.Any())
                {
                    data = data.Where(o => request.FilterTuple.OurCompanies.Contains((Guid)o.OurCompanyId!));
                }
                if (request.FilterTuple.ContragentsCompanies != null && request.FilterTuple.ContragentsCompanies.Any())
                {
                    data = data.Where(o => request.FilterTuple.ContragentsCompanies.Contains((Guid)o.SellersCompanyId!));
                }
            }
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                if (request.SearchInChapter!=null)
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
                                w.SellersCompany != null && w.SellersCompany.Name.ToLower().Contains(request.SearchString.ToLower()));
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
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Name);
                    break;
                case "number_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Id);
                    break;
            }
            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return Ok(new SortPagedResponse<ImportedOrder>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() });

        }

        [HttpPost]
        public async Task<IActionResult> Post(ImportedOrder importedOrder)
        {

            // _context.ImportedOrders.Add(importedOrder);
            _context.Entry(importedOrder).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return Ok(importedOrder.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ImportedOrder importedOrder)
        {
            //var findItem = await _context.ImportedOrders.FindAsync(importedOrder.Id);

            //if (findItem == null) return BadRequest($"Entity with id = {importedOrder.Id} was not found.");
            //findItem.Name = importedOrder.Name;
            //findItem.OurCompany = importedOrder.OurCompany;
            //findItem.OurEmployee = importedOrder.OurEmployee;
            //findItem.SellersCompany = importedOrder.SellersCompany;
            //findItem.EmployeeSeller = importedOrder.EmployeeSeller;

            
            _context.Entry(importedOrder).State = EntityState.Modified;
            //_context.Update(findItem);
            await _context.SaveChangesAsync();
            return Ok(importedOrder);
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ImportedOrder> importedOrders)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.ImportedOrders.UpdateRange(importedOrders);
            await _context.SaveChangesAsync();
            return Ok(importedOrders.Count());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = await _context.ImportedOrders.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return NoContent();

            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return Ok(dev);
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ImportedOrder> importedOrders)
        {
            _context.RemoveRange(importedOrders);
            await _context.SaveChangesAsync();
            return Ok(importedOrders.Count());
        }
    }
}
