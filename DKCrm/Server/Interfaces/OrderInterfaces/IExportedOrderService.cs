using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using System.Security.Claims;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IExportedOrderService
    {
        Task<IEnumerable<ExportedOrder>> GetAsync(ClaimsPrincipal user);
        Task<ExportedOrder> GetDetailAsync(Guid id, ClaimsPrincipal user);
        Task<SortPagedResponse<ExportedOrder>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderTuple> request, ClaimsPrincipal user);
        Task<Guid> PostAsync(ExportedOrder exportedOrder, string userName);
        Task<Guid> PutAsync(ExportedOrder exportedOrder, string userName);
        Task<int> PutRangeAsync(IEnumerable<ExportedOrder> exportedOrders);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<ExportedOrder> exportedOrders);
        Task<int> AddStatusToOrderAsync(ExportedOrderStatusExportedOrder exportedOrderStatus);
        Task<int> RemoveStatusFromOrderAsync(ExportedOrderStatusExportedOrder exportedOrderStatus);
    }
}
