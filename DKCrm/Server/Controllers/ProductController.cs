using DKCrm.Server.Data;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public ProductController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //var е =  _context.Products.ToList();
            return Ok(await _context.Products.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Product product)
        {
            var entityToBeUpdated = await _context.Products.FirstOrDefaultAsync(a => a.Id == product.Id);
            if (entityToBeUpdated == null)
                return BadRequest($"Entity with id = {product.Id} was not found.");
            entityToBeUpdated.Name = product.Name;
            entityToBeUpdated.PartNumber = product.PartNumber;
            entityToBeUpdated.Brand = product.Brand;
            entityToBeUpdated.Category = product.Category;

            //_context.Entry(product).State = EntityState.Modified;
            _context.Products.Update(entityToBeUpdated);
            await _context.SaveChangesAsync();
            return Ok(product);
        }
        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<Product> products)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.Products.UpdateRange(products);
            await _context.SaveChangesAsync();
            return Ok(products.Count());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = new Product { Id = id };
            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<Product> products)
        {
          _context.RemoveRange(products);
            await _context.SaveChangesAsync();
            return Ok(products.Count());
        }
    }
}
