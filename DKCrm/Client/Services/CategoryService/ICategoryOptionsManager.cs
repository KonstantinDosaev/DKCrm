using DKCrm.Shared.Models.Products;

namespace DKCrm.Client.Services.CategoryService
{
    public interface ICategoryOptionsManager
    {
        Task<List<CategoryOption>> GetAsync();
        Task<CategoryOption> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(CategoryOption option);
        Task<bool> AddAsync(CategoryOption option);
        Task<bool> RemoveAsync(Guid id);
    }
}
