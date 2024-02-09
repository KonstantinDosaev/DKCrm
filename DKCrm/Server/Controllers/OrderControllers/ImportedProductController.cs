using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models.Products;
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
            return Ok(await _context.ImportedProducts.Include(i => i.ImportedOrder)
                .Include(i => i.Product).IgnoreQueryFilters().AsSplitQuery().ToListAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var dev = await _context.ImportedProducts
                .Include(i=>i.ImportedOrder)
                .Include(i=>i.Product).ThenInclude(t=>t!.Brand).IgnoreQueryFilters().AsSingleQuery()
                .FirstOrDefaultAsync(a => a.Id == id);
            return Ok(dev);
        }
        [HttpGet]
        public async Task<IActionResult> GetNotEquipped()
        {
            var status = _context.ExportedOrderStatus.FirstOrDefault(f => f.Value == ImportOrderStatusNames.Delivery)!.Position;
            return Ok(await _context.ImportedProducts.Select(s => new
                {
                    s.Product,
                    s.Quantity,
                    s.ImportedOrder,
                    s.ImportedOrderId,
                    s.ProductId,
                    s.ExportedProducts,
                    s.PurchaseAtExportList,
                    s.PurchaseAtStorageList,
                    s.StorageList
                }).Where(w=>w.ImportedOrder!.ImportedOrderStatusImportedOrders!
                    .MaxBy(o => o.DateTimeCreate)!.ImportedOrderStatus!.Position < status)
                .Where(w => w.Quantity < w.PurchaseAtStorageList!.Select(s => s.Quantity).Sum() 
                    + w.PurchaseAtExportList!.Select(s => s.Quantity).Sum())
                .ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(ImportedProduct importedProduct)
        {
            _context.Entry(importedProduct).State = EntityState.Added;
            if (importedProduct.PurchaseAtStorageList != null)
            {
                foreach (var item in importedProduct.PurchaseAtStorageList)
                {
                    _context.Entry(item).State = EntityState.Added;
                }
            }
            if (importedProduct.PurchaseAtExportList != null)
            {
                foreach (var item in importedProduct.PurchaseAtExportList)
                {
                    _context.Entry(item).State = EntityState.Added;
                }
            }
            await _context.SaveChangesAsync();
            return Ok(importedProduct.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ImportedProduct importedProduct)
        {
            _context.Entry(importedProduct).State = EntityState.Modified;
          
            if (importedProduct.PurchaseAtStorageList != null)
            {
                var pas = await _context.PurchaseAtStorages.Where(w => w.ImportedProductId == importedProduct.Id).Select(s => s.StorageId).ToListAsync();
             
                foreach (var item in importedProduct.PurchaseAtStorageList)
                {
                    _context.Entry(item).State = pas.Contains(item.StorageId) ? EntityState.Modified : EntityState.Added;
                }
            }
            if (importedProduct.PurchaseAtExportList != null)
            {
                var pae = await _context.PurchaseAtExports.Where(w => w.ImportedProductId == importedProduct.Id).Select(s => s.ExportedProductId).ToListAsync();

                foreach (var item in importedProduct.PurchaseAtExportList)
                {
                    _context.Entry(item).State = pae.Contains(item.ExportedProductId) ? EntityState.Modified : EntityState.Added;
                }
            }
           
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
            var t = User.Identity!.Name;
            
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
