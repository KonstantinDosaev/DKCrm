using DKCrm.Server.Data;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImportedOrderStatusController:ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ImportedOrderStatusController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.ImportedOrderStatus.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.ImportedOrderStatus.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ImportedOrderStatus importedOrderStatus)
        {
            _context.Add(importedOrderStatus);
            await _context.SaveChangesAsync();
            return Ok(importedOrderStatus.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ImportedOrderStatus importedOrderStatus)
        {
            _context.Entry(importedOrderStatus).State = EntityState.Modified;
            //_context.Update(entityToBeUpdated);
            await _context.SaveChangesAsync();
            return Ok(importedOrderStatus);
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ImportedOrderStatus> importedOrderStatus)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.ImportedOrderStatus.UpdateRange(importedOrderStatus);
            await _context.SaveChangesAsync();
            return Ok(importedOrderStatus.Count());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = await _context.ImportedOrderStatus.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return NoContent();

            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return Ok(dev);
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ImportedOrderStatus> importedOrderStatus)
        {
            _context.RemoveRange(importedOrderStatus);
            await _context.SaveChangesAsync();
            return Ok(importedOrderStatus.Count());
        }
    }
}
