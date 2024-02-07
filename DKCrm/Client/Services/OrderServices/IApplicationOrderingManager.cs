using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IApplicationOrderingManager
    {
        Task<List<ApplicationOrderingProducts>> GetAsync();
        Task<ApplicationOrderingProducts> GetDetailsAsync(Guid id);
        Task<SortPagedResponse<ApplicationOrderingProducts>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterApplicationOrderTuple> request);
        Task<bool> UpdateAsync(ApplicationOrderingProducts item);
        Task<bool> AddAsync(ApplicationOrderingProducts item);
        Task<bool> RemoveRangeAsync(IEnumerable<ApplicationOrderingProducts> items);
        Task<bool> RemoveAsync(Guid id);
    }
}
