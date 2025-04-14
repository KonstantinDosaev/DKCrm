using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IInfoSetFromDocumentToOrderService
    {
        Task<InfoSetToDocument> GetOneAsync(Guid orderId);
        Task<IEnumerable<InfoSetToDocument>> GetAllInfoSetsDocumentsToOrderAsync(Guid orderId);
        Task<int> AddInfoSetToOrderAsync(InfoSetToDocument infoSetToDocument);
        Task<int> RemoveInfoSetFromOrderAsync(Guid id);
        Task<int> UpdateInfoSetToOrderAsync(InfoSetToDocument infoSetToDocument);
    }
}
