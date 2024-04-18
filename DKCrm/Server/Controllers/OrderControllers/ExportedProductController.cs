using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExportedProductController:ControllerBase
    {
        private readonly IExportedProductService _exportedProductService;
        public ExportedProductController(IExportedProductService exportedProductService)
        {
            _exportedProductService = exportedProductService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _exportedProductService.GetAsync());


        [HttpGet("{productId:guid}")]
        public async Task<IActionResult> GetNotEquipped(Guid productId) => Ok(await _exportedProductService.GetNotEquippedAsync(productId));

        [HttpGet]
        public async Task<IActionResult> GetAllNotEquippedAsync()=> Ok(await _exportedProductService.GetAllNotEquippedAsync());

        [HttpPost]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterExportedProductTuple> request)
            => Ok(await _exportedProductService.GetBySortPagedSearchChapterAsync(request));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _exportedProductService.GetOneAsync(id));

        [HttpPost]
        public async Task<IActionResult> Post(ExportedProduct exportedProduct) => Ok(await _exportedProductService.PostAsync(exportedProduct));

        [HttpPut]
        public async Task<IActionResult> Put(ExportedProduct exportedProduct) => Ok(await _exportedProductService.PutAsync(exportedProduct));

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _exportedProductService.DeleteAsync(id));

        [HttpPost]
        public async Task<IActionResult> UpdateSourcesOrderItems(ExportedProduct exportedProduct) => Ok(await _exportedProductService.UpdateSourcesOrderItems(exportedProduct));

        //[HttpPut("range")]
        //public async Task<IActionResult> PutRange(IEnumerable<ExportedProduct> exportedProducts)
        //{
        //    //_context.Entry(product).State = EntityState.Modified;
        //    _context.ExportedProducts.UpdateRange(exportedProducts);
        //    await _context.SaveChangesAsync();
        //    return Ok(exportedProducts.Count());
        //}

        //[HttpPost("removerange")]
        //public async Task<IActionResult> DeleteRange(IEnumerable<ExportedProduct> exportedProducts)
        //{
        //    _context.RemoveRange(exportedProducts);
        //    await _context.SaveChangesAsync();
        //    return Ok(exportedProducts.Count());
        //}
    }
}
