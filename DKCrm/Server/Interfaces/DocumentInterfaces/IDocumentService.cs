using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Server.Interfaces.DocumentInterfaces
{
    public interface IDocumentService
    {
        Task<byte[]> GetDocumentBytArrayAsync(Guid infoSetId);
        Task<int> RemoveDocumentAsync(Guid infoSetId);
        Task<bool> CreatePaymentInvoicePdfAsync(CreatePaymentInvoiceRequest request);
        Task<bool> CreateOrderSpecificationPdfAsync(CreateOrderSpecificationRequest createOrderSpecificationRequest);
        Task<byte[]> AddStampToPdfAsync(AddStampToPdfRequest request);
    }
}
