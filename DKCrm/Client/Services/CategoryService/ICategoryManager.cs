using DKCrm.Shared.Models.Products;

namespace DKCrm.Client.Services.CategoryService
{
    public interface ICategoryManager
    {
        Task<List<Category>> GetAsync();
        Task<Category> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(Category category);
        Task<bool> AddAsync(Category category);
        Task<bool> RemoveRangeAsync(IEnumerable<Category> categories);
        Task<bool> RemoveAsync( Guid id);
    }
}
