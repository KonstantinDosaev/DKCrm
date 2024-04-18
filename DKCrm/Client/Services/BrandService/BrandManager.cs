using DKCrm.Client.Constants;
using DKCrm.Shared.Models.Products;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.BrandService
{
    public class BrandManager : IBrandManager
    {
        private readonly HttpClient _httpClient;

        public BrandManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Brand>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Brand>>("api/brand") ?? throw new InvalidOperationException();
        }

        public async Task<Brand> GetDetailsAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<Brand>($"api/brand/{id}") ?? throw new InvalidOperationException();
        }

        public async Task UpdateAsync(Brand brand)
        {
            await _httpClient.PutAsJsonAsync("api/brand", brand, JsonOptions.JsonIgnore);
        }

        public async Task AddAsync(Brand brand)
        {
            await _httpClient.PostAsJsonAsync($"api/Brand", brand, JsonOptions.JsonIgnore);
        }

        public async Task RemoveRangeAsync(IEnumerable<Brand> brands)
        {
            await _httpClient.PostAsJsonAsync($"api/brand/removerange", brands, JsonOptions.JsonIgnore);
        }
    }
}
