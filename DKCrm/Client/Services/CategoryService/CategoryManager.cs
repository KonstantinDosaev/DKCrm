using DKCrm.Client.Constants;
using DKCrm.Shared.Models.Products;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.CategoryService
{
    public class CategoryManager : ICategoryManager
    {
        private readonly HttpClient _httpClient;

        public CategoryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Category>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Category>>("api/category") ?? throw new InvalidOperationException();
        }

        public async Task<Category> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Category>($"api/category/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            var result =await _httpClient.PutAsJsonAsync("api/category", category, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(Category category)
        {
           var result= await _httpClient.PostAsJsonAsync($"api/category", category, JsonOptions.JsonIgnore);
           return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<Category> categories)
        {
            var result=await _httpClient.PostAsJsonAsync($"api/category/removerange", categories, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
         public async Task<bool> RemoveAsync(Guid id)
        {
            var result=await _httpClient.DeleteAsync($"api/category/{id}");
            return result.IsSuccessStatusCode;
        }
    }
}
