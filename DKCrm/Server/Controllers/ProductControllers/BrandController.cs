using DKCrm.Server.Interfaces.ProductInterfaces;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;


namespace DKCrm.Server.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _brandService.GetAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _brandService.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(Brand brand)
        {
            return Ok(await _brandService.PostAsync(brand));
        }

        [HttpPut]
        public async Task<IActionResult> Put(Brand brand)
        {
            return Ok(await _brandService.PutAsync(brand));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _brandService.DeleteAsync(id));
        }
        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<Brand> brands)
        {
            return Ok(await _brandService.DeleteRangeAsync(brands));
        }
    }
}
