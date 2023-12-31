using DKCrm.Server.Data;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryOptionsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public CategoryOptionsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.CategoryOptions
                .Include(i => i.Category)
                .Include(i => i.ProductOption)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.CategoryOptions
                .Include(i => i.Category)
                .Include(i => i.ProductOption)
                .FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryOption option)
        {
            _context.CategoryOptions.Add(option);
            await _context.SaveChangesAsync();
            return Ok(option.Name);
        }

        [HttpPut]
        public async Task<IActionResult> Put(CategoryOption option)
        {
            var options = _context.CategoryOptions.Select(s=>s.Id);
            _context.Entry(option).State = options.Contains(option.Id) ? EntityState.Modified : EntityState.Added;
            await _context.SaveChangesAsync();
            return Ok(option);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = _context.CategoryOptions.FirstOrDefault(f => f.Id == id);

            if (dev != null) _context.CategoryOptions.Remove(dev);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
    }
}
