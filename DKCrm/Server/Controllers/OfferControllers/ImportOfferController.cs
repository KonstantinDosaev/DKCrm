using DKCrm.Server.Interfaces.ImportOfferInterfaces;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Server.Services.OrderServices;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OfferModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers.OfferControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ImportOfferController : ControllerBase
    {
        private readonly IImportOfferService _importOfferService;

        public ImportOfferController(IImportOfferService importOfferService)
        {
            _importOfferService = importOfferService;
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id) => Ok(await _importOfferService.GetDetailAsync(id, User));

        [HttpPost]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOfferTuple> request)
            => Ok(await _importOfferService.GetBySortPagedSearchChapterAsync(request, User));

        [HttpPost]
        public async Task<IActionResult> Post(ImportOffer offer)
            => Ok(await _importOfferService.PostAsync(offer));     
        [HttpPut]
        public async Task<IActionResult> Update(ImportOffer offer)
            => Ok(await _importOfferService.UpdateAsync(offer));

        [HttpPost]
        public async Task<IActionResult> UpdatePrice(PriceForImportOffer price)
            => Ok(await _importOfferService.UpdatePriceAsync(price));

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _importOfferService.DeleteAsync(id));       
        [HttpPost]
        public async Task<IActionResult> AddOfferToExportOrder(ExportProductPriceImportOffer link)
            => Ok(await _importOfferService.AddOfferToExportOrderAsync(link));   
    }
}
