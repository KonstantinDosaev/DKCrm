using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IImportedOrderService
    {
        Task<IEnumerable<ImportedOrder>> GetAsync();
        Task<ImportedOrder> GetDetailAsync(Guid id);
        Task<SortPagedResponse<ImportedOrder>> GetBySortPagedSearchChapterAsync(
            SortPagedRequest<FilterOrderTuple> request);

        Task<Guid> PostAsync(ImportedOrder importedOrder);
        Task<Guid> PutAsync(ImportedOrder importedOrder);
        Task<int> PutRangeAsync(IEnumerable<ImportedOrder> importedOrders);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<ImportedOrder> importedOrders);
        Task<int> AddStatusToOrderAsync(ImportedOrderStatusImportedOrder statusOrder);
    }
}
