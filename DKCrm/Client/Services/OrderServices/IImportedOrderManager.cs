using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IImportedOrderManager
    {
        Task<List<ImportedOrder>> GetAsync();
        Task<ImportedOrder> GetDetailsAsync(Guid id);
        Task<SortPagedResponse<ImportedOrder>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterOrderTuple> request);
        Task<bool> UpdateAsync(ImportedOrder item);
        Task<Guid> AddAsync(ImportedOrder item);
        Task<bool> RemoveRangeAsync(IEnumerable<ImportedOrder> items);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> AddStatusToOrderAsync(ImportedOrderStatusImportedOrder statusOrder);
    }
}
