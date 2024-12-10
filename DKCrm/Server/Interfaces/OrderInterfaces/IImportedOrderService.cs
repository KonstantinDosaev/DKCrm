using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using System.Security.Claims;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IImportedOrderService
    {
        Task<IEnumerable<ImportedOrder>> GetAsync(ClaimsPrincipal user);
        Task<ImportedOrder> GetDetailAsync(Guid id, ClaimsPrincipal user);
        Task<SortPagedResponse<ImportedOrder>> GetBySortPagedSearchChapterAsync(
            SortPagedRequest<FilterOrderTuple> request, ClaimsPrincipal user);

        Task<Guid> PostAsync(ImportedOrder importedOrder);
        Task<Guid> PutAsync(ImportedOrder importedOrder);
        Task<int> PutRangeAsync(IEnumerable<ImportedOrder> importedOrders);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<ImportedOrder> importedOrders);
        Task<int> AddStatusToOrderAsync(ImportedOrderStatusImportedOrder statusOrder);
    }
}
