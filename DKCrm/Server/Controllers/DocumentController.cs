using DKCrm.Server.Interfaces;
using DKCrm.Server.Interfaces.OrderInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DocumentController: ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IInfoSetFromDocumentToOrderService _infoSetFromDocumentToOrderService;
        public DocumentController(IDocumentService documentService, IInfoSetFromDocumentToOrderService infoSetFromDocumentToOrderService)
        {
            _documentService = documentService;
            _infoSetFromDocumentToOrderService = infoSetFromDocumentToOrderService;
        }

        [HttpGet("{infoSetId:guid}")]
        public async Task<IActionResult> GetDocumentBytArray(Guid infoSetId) 
            => Ok(await _documentService.GetDocumentBytArrayAsync(infoSetId));

        [HttpDelete("{infoSetId:guid}")]
        public async Task<IActionResult> RemoveDocument(Guid infoSetId) 
            => Ok(await _documentService.RemoveDocumentAsync(infoSetId));

        [HttpGet("{infoSetId:guid}")]
        public async Task<IActionResult> GetInfoSetFromDocumentFile(Guid infoSetId) 
            => Ok(await _infoSetFromDocumentToOrderService.GetOneAsync(infoSetId));

        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetAllDocumentFileInfoSetsForOrder(Guid orderId) 
            => Ok(await _infoSetFromDocumentToOrderService.GetAllInfoSetsDocumentsToOrderAsync(orderId));

        [HttpPost("{orderId:guid}")]
        public async Task<IActionResult> CreatePaymentInvoicePdf(Guid orderId) 
            => Ok(await _documentService.CreatePaymentInvoicePdfAsync(orderId));
    }
}
