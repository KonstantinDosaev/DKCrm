using DKCrm.Server.Interfaces.ProductInterfaces;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers.ProductControllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryOptionsController : ControllerBase
    {
        private readonly ICategoryOptionsService _categoryOptionsService;
        public CategoryOptionsController(ICategoryOptionsService categoryOptionsService)
        {
            _categoryOptionsService = categoryOptionsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _categoryOptionsService.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _categoryOptionsService.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryOption option)
        {
            return Ok(await _categoryOptionsService.PostAsync(option));
        }

        [HttpPut]
        public async Task<IActionResult> Put(CategoryOption option)
        {
            return Ok(await _categoryOptionsService.PutAsync(option));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _categoryOptionsService.DeleteAsync(id));
        }

    }
}
