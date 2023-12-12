using DKCrm.Server.Data;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                .Where(w => w.ImportedOrdersOurCompany!.Any() || w.ImportedOrdersSellersCompany!.Any()).LoadAsync();


            return Ok(await _context.ImportedOrders
                //.Include(i=>i.OurCompany).ThenInclude(t=>t!.Employees)
                //.Include(i=>i.SellersCompany).ThenInclude(t=>t!.Employees)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {

            //var dev = await _context.ImportedOrders
            //    .Include(f=>f.OurCompany)
            //    .Include(i=>i.OurEmployee)
            //    .Include(i=>i.SellersCompany)
            //    .Include(i=>i.EmployeeSeller)
            //    .FirstOrDefaultAsync(a => a.Id == id);
            var dev = await _context.ImportedOrders.FindAsync(id);
            await _context.Entry(dev!).Reference(u => u.SellersCompany).LoadAsync();
            await _context.Entry(dev!).Reference(u => u.EmployeeSeller).LoadAsync();
            await _context.Entry(dev!).Reference(u => u.OurCompany).LoadAsync();
            await _context.Entry(dev!).Reference(u => u.OurEmployee).LoadAsync();
            return Ok(dev);
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
