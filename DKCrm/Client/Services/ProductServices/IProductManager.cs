using DKCrm.Shared.Models.Products;

namespace DKCrm.Client.Services.ProductServices
{
    public interface IProductManager
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductDetailsAsync(string userId);
        Task UpdateProductAsync(Product user);
        Task AddProductAsync(Product user);
        Task RemoveRangeProductsAsync(IEnumerable<Product> user);
    }
}
