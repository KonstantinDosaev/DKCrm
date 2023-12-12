using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IImportedProductManager
    {
        Task<List<ImportedProduct>> GetAsync();
        Task<ImportedProduct> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(ImportedProduct item);
        Task<bool> AddAsync(ImportedProduct item);
        Task<bool> RemoveRangeAsync(IEnumerable<ImportedProduct> items);
        Task<bool> RemoveAsync(Guid id);
    }
}
