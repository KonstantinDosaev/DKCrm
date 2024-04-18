using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Requests;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IImportedProductManager
    {
        Task<List<ImportedProduct>> GetAsync();
        Task<ImportedProduct> GetDetailsAsync(Guid id);
        Task<List<ImportedProduct>> GetNotEquippedAsync(Guid productId);
        Task<List<ImportedProduct>> GetAllNotAddToOrderAsync();
        Task<bool> MergeImportedProductsAsync(MergeImportedProductsRequest request);
        Task<bool> UpdateAsync(ImportedProduct item);
        Task<bool> AddAsync(ImportedProduct item);
        Task<bool> RemoveRangeAsync(IEnumerable<ImportedProduct> items);
        Task<bool> RemoveAsync(Guid id);

        Task<bool> UpdateSourcesOrderItems(ImportedProduct item);
    }
}
