using DKCrm.Shared.Models.CompanyModels;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.CompanyServices
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly HttpClient _httpClient;

        public EmployeeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Employee>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Employee>>("api/Employee") ?? throw new InvalidOperationException();
        }

        public async Task<Employee> GetDetailsAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Employee>($"api/Employee/{id}") ?? throw new InvalidOperationException();
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            var result = await _httpClient.PutAsJsonAsync("api/Employee", employee);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddAsync(Employee employee)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Employee", employee);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/Employee/{id}");
            return result.IsSuccessStatusCode;
        }
    }
}
