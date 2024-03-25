using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IImportedProductService
    {
        Task<IEnumerable<ImportedProduct>> GetAsync();
        Task<IEnumerable<ImportedProduct>> GetNotEquippedAsync(Guid productId);
        Task<ImportedProduct> GetOneAsync(Guid id);
        Task<Guid> PostAsync(ImportedProduct importedProduct);
        Task<Guid> PutAsync(ImportedProduct importedProduct);
        Task<IEnumerable<Guid>> PutRangeAsync(IEnumerable<ImportedProduct> importedProduct);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<ImportedProduct> importedProduct);
    }
}
