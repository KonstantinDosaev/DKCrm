using DKCrm.Shared.Models.Products;

namespace DKCrm.Server.Interfaces.ProductInterfaces
{
    public interface ICategoryOptionsService
    {
        Task<IEnumerable<CategoryOption>> GetAsync();
        Task<CategoryOption> GetAsync(Guid id);
        Task<Guid> PostAsync(CategoryOption option);
        Task<Guid> PutAsync(CategoryOption option);
        Task<int> DeleteAsync(Guid id);
    }
}
