using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IDocumentToOrderService
    {
        Task<IEnumerable<InfoSetFromDocumentToOrder>> GetAllDocumentsToOrder(Guid orderId);
        Task<int> AddDocumentToOrder(InfoSetFromDocumentToOrder infoSetFromDocumentToOrder);
    }
}
