using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Server.Interfaces
{
    public interface IDocumentService
    {
        Task<byte[]> GetDocumentBytArrayAsync(Guid infoSetId);
        Task<int> RemoveDocumentAsync(Guid infoSetId);
        Task<bool> CreatePaymentInvoicePdfAsync(Guid orderId);
        Task<bool> CreateOrderSpecificationPdfAsync(CreateOrderSpecificationRequest createOrderSpecificationRequest);
    }
}
