using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExportedOrderStatusController : ControllerBase
    {
        private readonly IExportedOrderStatusServices _exportedOrderStatusServices;

        public ExportedOrderStatusController(IExportedOrderStatusServices exportedOrderStatusServices)
        {
            _exportedOrderStatusServices = exportedOrderStatusServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _exportedOrderStatusServices.GetAsync());
       
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _exportedOrderStatusServices.GetDetailAsync(id));
      
        [HttpPost]
        public async Task<IActionResult> Post(ExportedOrderStatus exportedOrderStatus)
            => Ok(await _exportedOrderStatusServices.PostAsync(exportedOrderStatus));

        [HttpPut]
        public async Task<IActionResult> Put(ExportedOrderStatus exportedOrderStatus)
            => Ok(await _exportedOrderStatusServices.PutAsync(exportedOrderStatus));

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ExportedOrderStatus> exportedOrderStatus)
            => Ok(await _exportedOrderStatusServices.PutRangeAsync(exportedOrderStatus));

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
            => Ok(await _exportedOrderStatusServices.DeleteAsync(id));

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ExportedOrderStatus> status)
            => Ok(await _exportedOrderStatusServices.DeleteRangeAsync(status));
    }
}
