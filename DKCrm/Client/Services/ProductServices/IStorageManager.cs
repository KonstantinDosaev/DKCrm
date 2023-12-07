using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.Products;

namespace DKCrm.Client.Services.ProductServices
{
    public interface IStorageManager
    {
        Task<List<Storage>> GetAsync();
        Task<Storage> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(Storage storage);
        Task<bool> AddAsync(Storage storage);
        Task<bool> RemoveAsync(Guid id);
    }
}
