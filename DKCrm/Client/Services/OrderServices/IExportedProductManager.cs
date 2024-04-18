using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IExportedProductManager
    {
        Task<List<ExportedProduct>> GetAsync();
        Task<List<ExportedProduct>> GetNotEquippedAsync(Guid productId);
        Task<List<ExportedProduct>> GetAllNotEquippedAsync();
        Task<ExportedProduct> GetDetailsAsync(Guid id);
        Task<SortPagedResponse<ExportedProduct>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterExportedProductTuple> request);
        Task<bool> UpdateAsync(ExportedProduct item);
        Task<bool> AddAsync(ExportedProduct item);
        Task<bool> RemoveRangeAsync(IEnumerable<ExportedProduct> items);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> UpdateSourcesOrderItems(ExportedProduct item);
    }
}
