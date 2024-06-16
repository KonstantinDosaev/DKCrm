using DKCrm.Client.Constants;
using System.Net.Http.Json;
using DKCrm.Shared.Models;

namespace DKCrm.Client.Services.CurrencyDictionaryService
{
    public class CurrencyDictionaryManager : ICurrencyDictionaryManager
    {
        private readonly HttpClient _httpClient;

        public CurrencyDictionaryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CurrencyDictionary>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CurrencyDictionary>>("api/CurrencyDictionary") ?? throw new InvalidOperationException();
        }

        public async Task<CurrencyDictionary> GetDetailsAsync(string charCode)
        {
            return await _httpClient.GetFromJsonAsync<CurrencyDictionary>($"api/CurrencyDictionary/{charCode}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(CurrencyDictionary currencyDictionary)
        {
            var result = await _httpClient.PutAsJsonAsync("api/CurrencyDictionary", currencyDictionary, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(CurrencyDictionary currencyDictionary)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/CurrencyDictionary", currencyDictionary, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/CurrencyDictionary/{id}");
            return result.IsSuccessStatusCode;
        }
    }
}
