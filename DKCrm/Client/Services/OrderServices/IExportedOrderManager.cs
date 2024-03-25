using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IExportedOrderManager
    {
        Task<List<ExportedOrder>> GetAsync();
        Task<ExportedOrder> GetDetailsAsync(Guid id);
        Task<SortPagedResponse<ExportedOrder>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterOrderTuple> request);
        Task<bool> UpdateAsync(ExportedOrder item);
        Task<bool> AddAsync(ExportedOrder item);
        Task<bool> RemoveRangeAsync(IEnumerable<ExportedOrder> items);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> AddStatusToOrderAsync(ExportedOrderStatusExportedOrder status);
        Task<bool> RemoveStatusFromOrderAsync(ExportedOrderStatusExportedOrder status);
    }
}
