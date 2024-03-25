using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models.Products;

namespace DKCrm.Server.Interfaces
{
    public interface IStorageService
    {
        Task<IEnumerable<Storage>> GetAsync();
        Task<Storage> GetAsync(Guid id);
        Task<IEnumerable<ProductsInStorage>> GetProductInStorageListAsync(Guid productId);
        Task<Guid> PostAsync(Storage storage);
        Task<Guid> PutAsync(Storage storage);
        Task<int> DeleteAsync(Guid id);
        Task<int> ReserveAProductAsync(SoldFromStorage soldFromStorage);
        Task<int> CancelReserveAProductAsync(SoldFromStorage soldFromStorage);
        
    }
}
