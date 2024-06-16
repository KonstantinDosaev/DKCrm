using System.Net.Http.Json;
using DKCrm.Shared.Models;
using DKCrm.Client.Constants;

namespace DKCrm.Client.Services.InternalCompanyDataService
{
    public class InternalCompanyDataManager: IInternalCompanyDataManager
    {
        private readonly HttpClient _httpClient;

        public InternalCompanyDataManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<InternalCompanyData> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync <InternalCompanyData>("api/InternalCompanyData") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(InternalCompanyData data)
        {
            var result = await _httpClient.PutAsJsonAsync("api/InternalCompanyData", data, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(InternalCompanyData data)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/InternalCompanyData", data, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
    }
}
