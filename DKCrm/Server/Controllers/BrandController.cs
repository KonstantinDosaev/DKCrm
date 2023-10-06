using DKCrm.Server.Data;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public BrandController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Brands.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.Brands.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return Ok(brand.Name);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Brand brand)
        {
            //var entityToBeUpdated = await _context.Products.FirstOrDefaultAsync(a => a.Id == product.Id);
            //if (entityToBeUpdated == null)
            //    return BadRequest($"Entity with id = {product.Id} was not found.");
            //entityToBeUpdated.Name = product.Name;
            //entityToBeUpdated.PartNumber = product.PartNumber;
            //entityToBeUpdated.Brand = product.Brand;
            //entityToBeUpdated.Category = product.Category;

            _context.Entry(brand).State = EntityState.Modified;
            //_context.Update(entityToBeUpdated);
            await _context.SaveChangesAsync();
            return Ok(brand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = new Brand { Id = id };
            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<Brand> brands)
        {
            _context.RemoveRange(brands);
            await _context.SaveChangesAsync();
            return Ok(brands.Count());
        }
    }
}
