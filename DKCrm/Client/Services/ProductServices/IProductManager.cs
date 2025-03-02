using DKCrm.Shared.Models;
using DKCrm.Shared.Models.Products;
using DKCrm.Shared.Requests;

namespace DKCrm.Client.Services.ProductServices
{
    public interface IProductManager
    {
        Task<List<Product>> GetProductsAsync();
       // Task<List<Product>> GetProductsByCategoryAsync(Guid? categoryId);
        Task<SortPagedResponse<ProductsDto>> GetProductsBySortAsync(SortPagedRequest<FilterProductTuple> request);
        Task<List<Product>> GetSearchProductAsync(string searchString);
        Task<Product> GetProductDetailsAsync(Guid id);
        Task<bool> UpdateProductAsync(Product user);
        Task <bool> UpdateRangeProductsAsync(IEnumerable<Product> products);
        Task AddProductAsync(Product user);
        Task RemoveRangeProductsAsync(IEnumerable<Product> products);
        Task<bool> RemoveAsync(DeleteForGuidRequest requestQ);
        Task<byte[]> OutputProductListToExcelAsync(List<Guid> productsIds);
        Task<List<string>> LoadProductsFromExcelAsync(byte[] excelBt);
    }
}
