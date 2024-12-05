using DKCrm.Shared.Models.CompanyModels;
using System.Net.Http.Json;
using DKCrm.Shared.Models;

namespace DKCrm.Client.Services.AccessRestrictionService
{
    public class AccessRestrictionManager : IAccessRestrictionManager
    {
        private readonly HttpClient _httpClient;

        public AccessRestrictionManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CheckExistAccessAndContainsUserInArrayToComponentAsync(Guid componentId)
        {
            return await _httpClient.GetFromJsonAsync<bool>
                ($"api/AccessRestriction/CheckExistAccessAndContainsUserInArrayToComponent/{componentId}");
        }
        public async Task<AccessRestriction> GetAccessFromComponentAsync(Guid componentId)
        {
            return await _httpClient.GetFromJsonAsync<AccessRestriction>
                ($"api/AccessRestriction/GetAccessFromComponent/{componentId}") ?? throw new InvalidOperationException();
        }
        public async Task<int> EditAccessToComponentAsync(AccessRestriction restriction)
        {
            var request = await _httpClient.PostAsJsonAsync("api/AccessRestriction/EditAccessToComponent", restriction);
            return await request.Content.ReadFromJsonAsync<int>();
        }
        public async Task<int> RemoveAccessAsync(Guid accessId)
        {
            var request = await _httpClient.DeleteAsync($"api/AccessRestriction/RemoveAccess/{accessId}");
            return await request.Content.ReadFromJsonAsync<int>();
        }
    }
    
}
