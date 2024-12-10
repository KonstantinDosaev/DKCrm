using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;


namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExportedOrderController:ControllerBase
    {
        private readonly IExportedOrderService _exportedOrderService;

        public ExportedOrderController(IExportedOrderService exportedOrderService)
        {
            _exportedOrderService = exportedOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _exportedOrderService.GetAsync(User));
        

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _exportedOrderService.GetDetailAsync(id, User));

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetOrder(Guid id)
        //{
        //    //var dev = await _context.ExportedOrders.Select(s => new
        //    //{
        //    //    s.Id,
        //    //    s.Name,
        //    //    s.OurCompany,
        //    //    s.CompanyBuyer,
        //    //    s.EmployeeBuyer,
        //    //    s.OurEmployee,
        //    //    s.EmployeeBuyerId,
        //    //    s.OurEmployeeId,
        //    //    s.ApplicationOrderingProducts,
        //    //    s.ExportedProducts,
        //    //}).FirstOrDefaultAsync(a => a.Id == id);
        //    //return Ok(dev);
        //    var result = await _context.ExportedOrders
        //        .Include(i => i.OurCompany).ThenInclude(i => i!.Employees)
        //        .Include(i => i.CompanyBuyer).ThenInclude(i => i!.Employees)
        //        .Include(i => i.ApplicationOrderingProducts).ThenInclude(t => t!.ProductList)
        //        .Include(i => i.ExportedProducts)!.ThenInclude(t => t.Product).ThenInclude(t => t!.Brand)
        //        .AsSingleQuery().FirstOrDefaultAsync(a => a.Id == id);
        //    return Ok(result);
        //}
        [HttpPost]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderTuple> request) 
            => Ok(await _exportedOrderService.GetBySortPagedSearchChapterAsync(request, User));

        [HttpPost]
        public async Task<IActionResult> Post(ExportedOrder exportedOrder) 
            => Ok(await _exportedOrderService.PostAsync(exportedOrder, User.Identity?.Name!));

        [HttpPut]
        public async Task<IActionResult> Put(ExportedOrder exportedOrder) 
            => Ok(await _exportedOrderService.PutAsync(exportedOrder, User.Identity?.Name!));

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ExportedOrder> exportedOrders) 
            => Ok(await _exportedOrderService.PutRangeAsync(exportedOrders));

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _exportedOrderService.DeleteAsync(id));

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ExportedOrder> exportedOrders) 
            => Ok(await _exportedOrderService.DeleteRangeAsync(exportedOrders));

        [HttpPost("add-status")]
        public async Task<IActionResult> AddStatusToOrder(ExportedOrderStatusExportedOrder exportedOrderStatus) 
            => Ok(await _exportedOrderService.AddStatusToOrderAsync(exportedOrderStatus));

        [HttpPost("remove-status")]
        public async Task<IActionResult> RemoveStatusFromOrder(ExportedOrderStatusExportedOrder exportedOrderStatus) 
            => Ok(await _exportedOrderService.RemoveStatusFromOrderAsync(exportedOrderStatus));
    }
}
