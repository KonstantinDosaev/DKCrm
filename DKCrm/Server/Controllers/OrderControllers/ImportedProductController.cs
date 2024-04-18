using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Server.Services.OrderServices;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImportedProductController : ControllerBase
    {
        private readonly IImportedProductService _importedProductService;

        public ImportedProductController(IImportedProductService importedProductService)
        {
            _importedProductService = importedProductService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _importedProductService.GetAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _importedProductService.GetOneAsync(id));

        [HttpGet("{productId:guid}")]
        public async Task<IActionResult> GetNotEquipped(Guid productId) => Ok(await _importedProductService.GetNotEquippedAsync(productId));

        [HttpGet]
        public async Task<IActionResult> GetAllNotAddToOrder() => Ok(await _importedProductService.GetAllNotAddToOrderAsync());

        [HttpPost]
        public async Task<IActionResult> Post(ImportedProduct importedProduct) => Ok(await _importedProductService.PostAsync(importedProduct));
       
        [HttpPut]
        public async Task<IActionResult> Put(ImportedProduct importedProduct) => Ok(await _importedProductService.PutAsync(importedProduct));

        [HttpPost]
        public async Task<IActionResult> MergeImportedProducts(MergeImportedProductsRequest request) => Ok(await _importedProductService.MergeImportedProductsAsync(request));

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ImportedProduct> importedProducts) => Ok(await _importedProductService.PutRangeAsync(importedProducts));

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _importedProductService.DeleteAsync(id));

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ImportedProduct> importedProducts) => Ok(await _importedProductService.DeleteRangeAsync(importedProducts));

        [HttpPost]
        public async Task<IActionResult> UpdateSourcesOrderItems(ImportedProduct importedProduct) => Ok(await _importedProductService.UpdateSourcesOrderItems(importedProduct));
        //[HttpPost("UpdatePurchaseAtExport")]
        //public async Task<IActionResult> UpdatePurchaseAtExport(PurchaseAtExport purchaseAtExport)
        //{
        //    var importedProduct = _context.ImportedProducts
        //        .Include(i=>i.PurchaseAtExportList)
        //        .Include(i=>i.PurchaseAtStorageList)
        //        .FirstOrDefault(i => i.Id == purchaseAtExport.ImportedProductId);

        //    var purchaseAtExportInDb = importedProduct.PurchaseAtExportList.FirstOrDefault(f =>
        //            f.ExportedProductId == purchaseAtExport.ExportedProductId && f.ImportedProductId == purchaseAtExport.ImportedProductId);
        //    var purchaseAtStorageInDb = importedProduct.PurchaseAtStorageList.FirstOrDefault(f =>
        //         f.ImportedProductId == purchaseAtExport.ImportedProductId);
        //    if (importedProduct != null)
        //    {
        //        if (purchaseAtExportInDb != null)
        //        {
        //            if (purchaseAtExportInDb.Quantity == purchaseAtExport.Quantity)
        //                return BadRequest();

        //            int modQuantity;
        //            if (purchaseAtExportInDb.Quantity < purchaseAtExport.Quantity)
        //            {
        //                modQuantity = purchaseAtExport.Quantity - purchaseAtExportInDb.Quantity;
        //                importedProduct.Quantity -= modQuantity;
        //            }
        //            else
        //            {
        //                modQuantity = purchaseAtExportInDb.Quantity - purchaseAtExport.Quantity;
        //                importedProduct.Quantity += modQuantity;
        //            }
        //        }
        //        else
        //        {
        //            importedProduct.Quantity -= soldFromStorage.Quantity;
        //        }

        //        if (importedProduct.Quantity < 0)
        //            return BadRequest();

        //        _context.Entry(importedProduct).State = EntityState.Modified;
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //    _context.Entry(soldFromStorage).State = purchaseAtExportInDb != null ? EntityState.Modified : EntityState.Added;

        //    await _context.SaveChangesAsync();
        //    return Ok(soldFromStorage.ExportedProductId);
        //}
    }
}
