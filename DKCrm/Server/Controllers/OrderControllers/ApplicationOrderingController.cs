using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApplicationOrderingController : ControllerBase
    {
        private readonly IApplicationOrderingService _applicationOrderingService;

        public ApplicationOrderingController(IApplicationOrderingService applicationOrderingService)
        {
            _applicationOrderingService = applicationOrderingService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _applicationOrderingService.GetAsync());
       
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _applicationOrderingService.GetDetailAsync(id));
        
        [HttpPost]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterApplicationOrderTuple> request)
            => Ok(await _applicationOrderingService.GetBySortPagedSearchChapterAsync(request));

        [HttpPost]
        public async Task<IActionResult> Post(ApplicationOrderingProducts order)
            => Ok(await _applicationOrderingService.PostAsync(order));

        [HttpPut]
        public async Task<IActionResult> Put(ApplicationOrderingProducts order)
            => Ok(await _applicationOrderingService.PutAsync(order));

        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<ApplicationOrderingProducts> orders)
            => Ok(await _applicationOrderingService.PutRangeAsync(orders));

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
            => Ok(await _applicationOrderingService.DeleteAsync(id));

        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<ApplicationOrderingProducts> orders)
            => Ok(await _applicationOrderingService.DeleteRangeAsync(orders));
    }
}
