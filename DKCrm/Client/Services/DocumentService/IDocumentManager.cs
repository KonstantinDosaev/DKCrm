using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.DocumentService
{
    public interface IDocumentManager
    {
        Task<IEnumerable<InfoSetFromDocumentToOrder>> GetAllInfoSetsForOrderAsync(Guid orderId);
        Task<IEnumerable<InfoSetFromDocumentToOrder>> GetOneInfoSetFromDocumentFileAsync(Guid infoSetId);
        Task<byte[]> GetDocumentBytArrayAsync(Guid infoSetId);
        Task<bool> RemoveDocumentAsync(Guid infoSetId);
         Task<bool> CreatePaymentInvoicePdfAsync(Guid orderId);
    }
}
