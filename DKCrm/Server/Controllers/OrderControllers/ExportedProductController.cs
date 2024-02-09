using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExportedProductController:ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ExportedProductController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.ExportedProducts.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> GetNotEquipped()
        {
            var status = _context.ExportedOrderStatus.FirstOrDefault(f => f.Value == ExportOrderStatusNames.Delivery)!.Position;
            return Ok(await _context.ExportedProducts.Select(s=> new
            {
                s.Product,
                s.Quantity,
                s.ExportedOrder,
                s.ExportedOrderId,
                s.ProductId,
                s.ImportedProducts,
                s.PurchaseAtExports,
                s.SoldFromStorage,
                s.StorageList
            }).Where(w=>w.ExportedOrder!.ExportedOrderStatusExported!
                    .MaxBy(o=>o.DateTimeCreate)!.ExportedOrderStatus!.Position < status)
                .Where(w=>w.Quantity< w.SoldFromStorage!.Select(s=>s.Quantity).Sum() + w.PurchaseAtExports!.Select(s=>s.Quantity).Sum())
                .ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.ExportedProducts
                .Include(i => i.ExportedOrder)
                .Include(i => i.Product).ThenInclude(t => t!.Brand).IgnoreQueryFilters().AsSingleQuery()
                .FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ExportedProduct exportedProduct)
        {
            _context.Add(exportedProduct);
            await _context.SaveChangesAsync();
            return Ok(exportedProduct.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ExportedProduct exportedProduct)
        {
            _context.Entry(exportedProduct).State = EntityState.Modified;
            //_context.Update(entityToBeUpdated);
            await _context.SaveChangesAsync();
            return Ok(exportedProduct);
        }

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ExportedProduct> exportedProducts)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.ExportedProducts.UpdateRange(exportedProducts);
            await _context.SaveChangesAsync();
            return Ok(exportedProducts.Count());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dev = await _context.ExportedProducts.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return NoContent();

            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return Ok(dev);
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ExportedProduct> exportedProducts)
        {
            _context.RemoveRange(exportedProducts);
            await _context.SaveChangesAsync();
            return Ok(exportedProducts.Count());
        }
    }
}
