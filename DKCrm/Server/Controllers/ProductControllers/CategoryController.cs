using DKCrm.Server.Interfaces.ProductInterfaces;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _categoryService.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _categoryService.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(Category category)
        {
            return Ok(await _categoryService.PostAsync(category));
        }

        [HttpPut]
        public async Task<IActionResult> Put(Category category)
        {
            return Ok(await _categoryService.PutAsync(category));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _categoryService.DeleteAsync(id));
        }

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<Category> categories)
        {
            return Ok(await _categoryService.DeleteRangeAsync(categories));
        }
    }
}
