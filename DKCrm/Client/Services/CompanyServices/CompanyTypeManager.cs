using DKCrm.Client.Constants;
using DKCrm.Shared.Models.CompanyModels;
using System.Data;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.CompanyServices
{
    public class CompanyTypeManager: ICompanyTypeManager
    {
        private readonly HttpClient _httpClient;

        public CompanyTypeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<CompanyType>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CompanyType>>("api/companyType") ?? throw new InvalidOperationException();
        }
    

        public async Task<CompanyType> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<CompanyType>($"api/companyType/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(CompanyType companyType)
        {
            var result = await _httpClient.PutAsJsonAsync("api/companyType", companyType, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(CompanyType companyType)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/companyType", companyType, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<CompanyType> companyTypes)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/companyType/removerange", companyTypes, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/companyType/{id}");
            return result.IsSuccessStatusCode;
        }
    }
}
