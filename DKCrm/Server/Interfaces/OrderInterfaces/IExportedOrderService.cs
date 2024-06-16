using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IExportedOrderService
    {
        Task<IEnumerable<ExportedOrder>> GetAsync();
        Task<ExportedOrder> GetDetailAsync(Guid id);
        Task<SortPagedResponse<ExportedOrder>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderTuple> request);
        Task<Guid> PostAsync(ExportedOrder exportedOrder, string userName);
        Task<Guid> PutAsync(ExportedOrder exportedOrder, string userName);
        Task<int> PutRangeAsync(IEnumerable<ExportedOrder> exportedOrders);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<ExportedOrder> exportedOrders);
        Task<int> AddStatusToOrderAsync(ExportedOrderStatusExportedOrder exportedOrderStatus);
        Task<int> RemoveStatusFromOrderAsync(ExportedOrderStatusExportedOrder exportedOrderStatus);
    }
}
