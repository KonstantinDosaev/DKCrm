using System.Net.Http.Json;
using DKCrm.Shared.Models.Products;

namespace DKCrm.Client.Services.ProductServices
{
    public class ProductManager : IProductManager
    {
        private readonly HttpClient _httpClient;

        public ProductManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Product>> GetProductsAsync()
        {
           return await _httpClient.GetFromJsonAsync<List<Product>>("api/product") ?? throw new InvalidOperationException();
        }

        public async Task<Product> GetProductDetailsAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"api/product/{userId}") ?? throw new InvalidOperationException();
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _httpClient.PutAsJsonAsync("api/product", product);
        }

        public async Task AddProductAsync(Product product)
        {
            await _httpClient.PostAsJsonAsync($"api/product", product);
        }

        public async Task RemoveRangeProductsAsync(IEnumerable<Product> user)
        {
             await _httpClient.DeleteAsync($"api/product/removerange");
        }
    }
}
