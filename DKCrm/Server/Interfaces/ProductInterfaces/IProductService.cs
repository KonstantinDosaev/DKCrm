using DKCrm.Shared.Models.Products;
using DKCrm.Shared.Models;

namespace DKCrm.Server.Interfaces.ProductInterfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAsync();
        Task<Product> GetDetailAsync(Guid id);

        Task<SortPagedResponse<ProductsDto>> GetBySortPagedSearchChapterAsync(
            SortPagedRequest<FilterProductTuple> request);

        Task<IEnumerable<Product>> GetSearchAsync(string searchString);
        Task<Guid> PostAsync(Product product);
        Task<Guid> PutAsync(Product product);
        Task<int> PutRangeAsync(IEnumerable<Product> products);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<Guid> products);
        
    }
}
