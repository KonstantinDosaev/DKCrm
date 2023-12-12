using DKCrm.Server.Data;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return Ok(await _context.ExportedOrders.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.ExportedOrders.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ExportedOrder exportedOrder)
        {
            _context.Add(exportedOrder);
            await _context.SaveChangesAsync();
            return Ok(exportedOrder.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ExportedOrder exportedOrder)
        {
            _context.Entry(exportedOrder).State = EntityState.Modified;
            //_context.Update(entityToBeUpdated);
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
