using DKCrm.Shared.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;

namespace DKCrm.Client.Services.UserService
{
    public class UserManagerCustom: IUserManagerCustom
    {
        private readonly HttpClient _httpClient;

        public UserManagerCustom(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
       
        public async Task<ApplicationUser> GetUserDetailsAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<ApplicationUser>($"api/user/{userId}");
        }

        public async Task UpdateUser(ApplicationUser user)
        {
             await _httpClient.PostAsJsonAsync("api/user/update",user);
        }

        public async Task AddUser(ApplicationUser user)
        { 
            await _httpClient.PostAsJsonAsync("api/user/add", user);
        }

        public async Task RemoveUsers(IEnumerable<ApplicationUser> users)
        {
            await _httpClient.PostAsJsonAsync("api/user/removerange", users);
        }


        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<ApplicationUser>>("api/user")); 
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return (await _httpClient.GetFromJsonAsync<List<IdentityRole>>("api/user/roleAll"));
        }

        public Task<IdentityRole> GetRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetRoleFromUser(string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<string>>($"api/user/rolefromuser/{userId}");
        }

        public async Task AddUserToRole(RoleRequest request)
        {
            await _httpClient.PostAsJsonAsync("api/user/addtorole", request);
        }

        public async Task UpdateUserRole(RoleRequest request)
        {
            await _httpClient.PostAsJsonAsync("api/user/updateuserrole", request);
        }

        //public async Task AddUserToRoles(RolesRequest request)
        //{
        //    await _httpClient.PostAsJsonAsync("api/user/addtoroles", request);
        //}

        //public async Task AddRole(string roleName)
        //{
        //    await _httpClient.PostAsJsonAsync($"api/user/addrole/{roleName}", roleName);
        //}
    }
}
