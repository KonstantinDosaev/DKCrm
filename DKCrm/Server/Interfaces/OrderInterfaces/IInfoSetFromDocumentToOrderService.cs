using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IInfoSetFromDocumentToOrderService
    {
        Task<InfoSetFromDocumentToOrder> GetOneAsync(Guid orderId);
        Task<IEnumerable<InfoSetFromDocumentToOrder>> GetAllInfoSetsDocumentsToOrderAsync(Guid orderId);
        Task<int> AddInfoSetToOrderAsync(InfoSetFromDocumentToOrder infoSetFromDocumentToOrder);
        Task<int> RemoveInfoSetFromOrderAsync(Guid id);
        Task<int> UpdateInfoSetToOrderAsync(InfoSetFromDocumentToOrder infoSetFromDocumentToOrder);
    }
}
