using DKCrm.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DocumentController: ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoc()
        {
            var t = await _documentService.CreateAsync();
            return Ok(t);
        }
        [HttpPost("{orderId:guid}")]
        public async Task<IActionResult> CreatePaymentInvoicePdf(Guid orderId)
        {
            var t = await _documentService.CreatePaymentInvoicePdfAsync(orderId);
            return Ok(t);
        }
    }
}
