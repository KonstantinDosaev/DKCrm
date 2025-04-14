using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Requests.FileService;

namespace DKCrm.Client.Services.DocumentService
{
    public interface IDocumentManager
    {
        Task<IEnumerable<InfoSetToDocument>> GetAllInfoSetsForOrderAsync(Guid orderId);
        Task<IEnumerable<InfoSetToDocument>> GetOneInfoSetFromDocumentFileAsync(Guid infoSetId);
        Task<byte[]> GetDocumentBytArrayAsync(Guid infoSetId);
        Task<bool> RemoveDocumentAsync(Guid infoSetId);
         Task<bool> CreatePaymentInvoicePdfAsync(CreatePaymentInvoiceRequest request);
         Task<bool> CreateOrderSpecificationPdfAsync(CreateOrderSpecificationRequest request);
         Task<byte[]> AddStampToPdfAsync(AddStampToPdfRequest request);
         Task<bool> CreateCommercialOfferPdfAsync(CreateCommercialOfferPdfRequest request);
        Task<bool> UploadDocumentFileAsync(UploadDocumentRequest request);
    }
}
