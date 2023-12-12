using DKCrm.Server.Data;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImportedProductController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ImportedProductController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.ImportedProducts.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.ImportedProducts.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ImportedProduct importedProduct)
        {
            _context.Add(importedProduct);
            await _context.SaveChangesAsync();
            return Ok(importedProduct.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ImportedProduct importedProduct)
        {
            _context.Entry(importedProduct).State = EntityState.Modified;
            //_context.Update(entityToBeUpdated);
            await _context.SaveChangesAsync();
            return Ok(importedProduct);
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ImportedProduct> importedProducts)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.ImportedProducts.UpdateRange(importedProducts);
            await _context.SaveChangesAsync();
            return Ok(importedProducts.Count());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = await _context.ImportedProducts.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return NoContent();

            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return Ok(dev);
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ImportedProduct> importedProducts)
        {
            _context.RemoveRange(importedProducts);
            await _context.SaveChangesAsync();
            return Ok(importedProducts.Count());
        }
    }
}
