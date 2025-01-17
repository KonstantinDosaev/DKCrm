using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImportedOrderController : ControllerBase
    {
        private readonly IImportedOrderService _importedOrderService;

        public ImportedOrderController(IImportedOrderService importedOrderService)
        {
            _importedOrderService = importedOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _importedOrderService.GetAsync(User));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _importedOrderService.GetDetailAsync(id, User));

        [HttpPost]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderTuple> request)
            => Ok(await _importedOrderService.GetBySortPagedSearchChapterAsync(request, User));

        [HttpPost]
        public async Task<IActionResult> Post(ImportedOrder importedOrder) => Ok(await _importedOrderService.PostAsync(importedOrder));

        [HttpPut]
        public async Task<IActionResult> Put(ImportedOrder importedOrder) => Ok(await _importedOrderService.PostAsync(importedOrder));

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ImportedOrder> importedOrders) => Ok(await _importedOrderService.PutRangeAsync(importedOrders));

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _importedOrderService.DeleteAsync(id));

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ImportedOrder> importedOrders)
            => Ok(await _importedOrderService.DeleteRangeAsync(importedOrders));

        [HttpPost("add-status")]
        public async Task<IActionResult> AddStatusToOrder(ImportedOrderStatusImportedOrder statusImportedOrder)
            => Ok(await _importedOrderService.AddStatusToOrderAsync(statusImportedOrder, User));
    }
}
