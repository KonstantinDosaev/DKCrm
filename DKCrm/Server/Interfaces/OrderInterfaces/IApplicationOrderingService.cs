using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IApplicationOrderingService
    {
        Task<IEnumerable<ApplicationOrderingProducts>> GetAsync();
        Task<ApplicationOrderingProducts> GetDetailAsync(Guid id);

        Task<SortPagedResponse<ApplicationOrderingProducts>> GetBySortPagedSearchChapterAsync(
            SortPagedRequest<FilterApplicationOrderTuple> request);
        Task<Guid> PostAsync(ApplicationOrderingProducts applicationOrderingProduct);
        Task<Guid> PutAsync(ApplicationOrderingProducts applicationOrderingProduct);
        Task<int> PutRangeAsync(IEnumerable<ApplicationOrderingProducts> applicationOrderingProducts);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<ApplicationOrderingProducts> applicationOrderingProducts);
    }
}
