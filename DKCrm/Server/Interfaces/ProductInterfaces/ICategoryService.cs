using DKCrm.Shared.Models.Products;

namespace DKCrm.Server.Interfaces.ProductInterfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAsync();
        Task<Category> GetAsync(Guid id);
        Task<Guid> PostAsync(Category category);
        Task<Guid> PutAsync(Category category);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<Category> categories);
       
    }
}
