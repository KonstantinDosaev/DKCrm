using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Requests;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IImportedProductService
    {
        Task<IEnumerable<ImportedProduct>> GetAsync();
        Task<IEnumerable<ImportedProduct>> GetNotEquippedAsync(Guid productId);
        Task<IEnumerable<ImportedProduct>> GetAllNotAddToOrderAsync();
        Task<ImportedProduct> GetOneAsync(Guid id);
        Task<Guid> PostAsync(ImportedProduct importedProduct);
        Task<Guid> PutAsync(ImportedProduct importedProduct);
        Task<int> UpdateSourcesOrderItems(ImportedProduct importedProduct);
        Task<Guid> MergeImportedProductsAsync(MergeImportedProductsRequest mergeRequest);
        Task<IEnumerable<Guid>> PutRangeAsync(IEnumerable<ImportedProduct> importedProduct);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<ImportedProduct> importedProduct);
    }
}
