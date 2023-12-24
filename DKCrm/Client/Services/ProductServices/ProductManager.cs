using System.Net;
using System.Net.Http.Json;
using DKCrm.Shared.Models;
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

        //public async Task<List<Product>> GetProductsByCategoryAsync(Guid? categoryId)
        //{
        //    return await _httpClient.GetFromJsonAsync<List<Product>>($"api/Product/category/{categoryId}") ?? throw new InvalidOperationException();
        //}
        public async Task<SortPagedResponse<ProductsDto>> GetProductsBySortAsync(SortPagedRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Product/category/",request) ?? throw new InvalidOperationException();
            return await response.Content.ReadFromJsonAsync<SortPagedResponse<ProductsDto>>() ?? throw new InvalidOperationException();

        }

        public async Task<List<Product>> GetSearchProductAsync(string searchString)
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>($"api/Product/{searchString}") ?? throw new InvalidOperationException();
        }

        public async Task<Product> GetProductDetailsAsync(Guid userId)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"api/product/{userId}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
           var result = await _httpClient.PutAsJsonAsync("api/product", product);
            return result.StatusCode== HttpStatusCode.OK; 
        }

        public async Task<bool> UpdateRangeProductsAsync(IEnumerable<Product> products)
        {
           return (await _httpClient.PutAsJsonAsync("api/product/range", products)).StatusCode== HttpStatusCode.OK;
        }    
        public async Task<bool> RemoveAsync(Guid id)
        {
           return (await _httpClient.DeleteAsync($"api/product/{id}")).StatusCode== HttpStatusCode.OK;
        }

        public async Task AddProductAsync(Product product)
        {
            await _httpClient.PostAsJsonAsync($"api/product", product);
        }

        public async Task RemoveRangeProductsAsync(IEnumerable<Product> products)
        {
             await _httpClient.PostAsJsonAsync($"api/product/removerange",products);
        }
    }
}
