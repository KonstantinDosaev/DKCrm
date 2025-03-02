using DKCrm.Shared.Models.Products;
using DKCrm.Shared.Models;
using DKCrm.Shared.Requests;

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
        Task<int> DeleteAsync(DeleteForGuidRequest request);
        Task<int> DeleteRangeAsync(IEnumerable<Guid> products);
        Task<byte[]> OutputProductListToExcelAsync(List<Guid> productsIds);
        Task<List<string>> LoadProductsFromExcelAsync(byte[] excelBt);
    }
}
