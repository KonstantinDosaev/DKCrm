using DKCrm.Shared.Models.Products;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.CategoryService
{
    public class CategoryOptionsManager: ICategoryOptionsManager
    {
        private readonly HttpClient _httpClient;

        public CategoryOptionsManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<CategoryOption>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryOption>>("api/CategoryOptions") ?? throw new InvalidOperationException();
        }

        public async Task<CategoryOption> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<CategoryOption>($"api/CategoryOptions/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(CategoryOption option)
        {
            var result = await _httpClient.PutAsJsonAsync("api/CategoryOptions", option);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(CategoryOption option)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/CategoryOptions", option);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/CategoryOptions/{id}");
            return result.IsSuccessStatusCode;
        }
    }
}
