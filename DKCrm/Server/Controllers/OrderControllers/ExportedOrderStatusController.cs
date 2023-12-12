using DKCrm.Server.Data;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExportedOrderStatusController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ExportedOrderStatusController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.ExportedOrderStatus.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.ExportedOrderStatus.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ExportedOrderStatus exportedOrderStatus)
        {
            _context.Add(exportedOrderStatus);
            await _context.SaveChangesAsync();
            return Ok(exportedOrderStatus.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ExportedOrderStatus exportedOrderStatus)
        {
            _context.Entry(exportedOrderStatus).State = EntityState.Modified;
            //_context.Update(entityToBeUpdated);
            await _context.SaveChangesAsync();
            return Ok(exportedOrderStatus);
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ExportedOrderStatus> exportedOrderStatus)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.ExportedOrderStatus.UpdateRange(exportedOrderStatus);
            await _context.SaveChangesAsync();
            return Ok(exportedOrderStatus.Count());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = await _context.ExportedOrderStatus.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return NoContent();

            _context.ExportedOrderStatus.Remove(dev);
            await _context.SaveChangesAsync();
            return Ok(dev);
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<TagsCompany> tagsCompanies)
        {
            _context.RemoveRange(tagsCompanies);
            await _context.SaveChangesAsync();
            return Ok(tagsCompanies.Count());
        }
    }
}
