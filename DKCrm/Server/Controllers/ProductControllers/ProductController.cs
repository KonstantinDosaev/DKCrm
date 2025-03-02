using DKCrm.Server.Interfaces.ProductInterfaces;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.Products;
using DKCrm.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Threading.Tasks;

namespace DKCrm.Server.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _productService.GetAsync());

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _productService.GetDetailAsync(id));

        [HttpPost("get-sort-filtered")]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterProductTuple> request)
            => Ok(await _productService.GetBySortPagedSearchChapterAsync(request));

        [HttpGet("{searchString}")]
        public async Task<IActionResult> GetSearch(string searchString) => Ok(await _productService.GetSearchAsync(searchString));

        [HttpPost]
        public async Task<IActionResult> Post(Product product) => Ok(await _productService.PostAsync(product));

        [HttpPut]
        public async Task<IActionResult> Put(Product product) => Ok(await _productService.PutAsync(product));

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<Product> products) => Ok(await _productService.PutRangeAsync(products));

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(DeleteForGuidRequest request) => Ok(await _productService.DeleteAsync(request));

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<Guid> products) => Ok(await _productService.DeleteRangeAsync(products));

        [HttpPost("outputXl")]
        public async Task<IActionResult> OutputProductListToExcel(List<Guid> productsIds)
            => Ok(await _productService.OutputProductListToExcelAsync(productsIds));   
        
        [HttpPost("loadFromXl")]
        public async Task<IActionResult> LoadProductsFromExcel(byte[] excelBt)
            => Ok(await _productService.LoadProductsFromExcelAsync(excelBt));
    }
}
