using DKCrm.Server.Data;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public CategoryController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Categories.Include(i=>i.Parent).Include(i=>i.Children).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.Categories.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category.Name);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Category category)
        {
            //var entityToBeUpdated = await _context.Products.FirstOrDefaultAsync(a => a.Id == product.Id);
            //if (entityToBeUpdated == null)
            //    return BadRequest($"Entity with id = {product.Id} was not found.");
            //entityToBeUpdated.Name = product.Name;
            //entityToBeUpdated.PartNumber = product.PartNumber;
            //entityToBeUpdated.Brand = product.Brand;
            //entityToBeUpdated.Category = product.Category;

            _context.Entry(category).State = EntityState.Modified;
            //_context.Update(entityToBeUpdated);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = _context.Categories.FirstOrDefault(f=>f.Id==id);
     
            _context.Categories.Remove(dev);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<Category> categories)
        {
            _context.RemoveRange(categories);
            await _context.SaveChangesAsync();
            return Ok(categories.Count());
        }
    }
}
