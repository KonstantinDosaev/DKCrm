using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImportedOrderStatusController:ControllerBase
    {
        private readonly IImportedOrderStatusService _importedOrderStatusService;

        public ImportedOrderStatusController(IImportedOrderStatusService importedOrderStatusService)
        {
            _importedOrderStatusService = importedOrderStatusService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _importedOrderStatusService.GetAsync());
  
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _importedOrderStatusService.GetDetailAsync(id));
     
        [HttpPost]
        public async Task<IActionResult> Post(ImportedOrderStatus importedOrderStatus) 
            => Ok(await _importedOrderStatusService.PostAsync(importedOrderStatus));
    
        [HttpPut]
        public async Task<IActionResult> Put(ImportedOrderStatus importedOrderStatus)
            => Ok(await _importedOrderStatusService.PutAsync(importedOrderStatus));
      
        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ImportedOrderStatus> importedOrderStatus)
            => Ok(await _importedOrderStatusService.PutRangeAsync(importedOrderStatus));

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
            => Ok(await _importedOrderStatusService.DeleteAsync(id));

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ImportedOrderStatus> importedOrderStatus)
            => Ok(await _importedOrderStatusService.DeleteRangeAsync(importedOrderStatus));
    }
}
