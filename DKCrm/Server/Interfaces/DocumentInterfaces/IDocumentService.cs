using System.Security.Claims;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Server.Interfaces.DocumentInterfaces
{
    public interface IDocumentService
    {
        Task<byte[]> GetDocumentBytArrayAsync(Guid infoSetId);
        Task<int> RemoveDocumentAsync(Guid infoSetId);
        Task<bool> CreatePaymentInvoicePdfAsync(CreatePaymentInvoiceRequest request, ClaimsPrincipal user);
        Task<bool> CreateOrderSpecificationPdfAsync(CreateOrderSpecificationRequest createOrderSpecificationRequest, ClaimsPrincipal user);
        Task<byte[]> AddStampToPdfAsync(AddStampToPdfRequest request);
    }
}
