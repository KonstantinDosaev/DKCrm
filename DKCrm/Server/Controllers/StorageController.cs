using DKCrm.Server.Interfaces;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _storageService.GetAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _storageService.GetAsync(id));
        }

        [HttpGet("productInStorageList/{productId:guid}")]
        public async Task<IActionResult> GetProductInStorageListAsync(Guid productId)
        {
            return Ok(await _storageService.GetProductInStorageListAsync(productId));
        }
        [HttpPost]
        public async Task<IActionResult> Post(Storage storage)
        {
            return Ok(await _storageService.PostAsync(storage));
        }

        [HttpPut]
        public async Task<IActionResult> Put(Storage storage)
        {
            return Ok(await _storageService.PutAsync(storage));
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _storageService.DeleteAsync(id));
        }

        [HttpPost("ReserveAProduct")]
        public async Task<IActionResult> ReserveAProduct(SoldFromStorage soldFromStorage)
        {
            return Ok(await _storageService.ReserveAProductAsync(soldFromStorage));
        }
        [HttpPost("CancelReserveAProduct")]
        public async Task<IActionResult> CancelReserveAProduct(SoldFromStorage soldFromStorage)
        {
            return Ok(await _storageService.CancelReserveAProductAsync(soldFromStorage));
        }
    }
}
