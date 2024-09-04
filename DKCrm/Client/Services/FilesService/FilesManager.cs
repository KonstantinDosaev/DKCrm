using DKCrm.Client.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Requests.FileService;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.FilesService
{
    public class FilesManager : IFilesManager
    {
        private readonly HttpClient _httpClient;

        public FilesManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<string>> GetAllFileNamesInDirectoryAsync(GetFileRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Files/GetAllFileNamesInDirectory", request, JsonOptions.JsonIgnore);
            return await result.Content.ReadFromJsonAsync<IEnumerable<string>>() ?? throw new InvalidOperationException();
        }
        public async Task<Dictionary<string, string>> GetAllFileNamesAndPathsInDirectoryAsync(GetFileRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Files/GetAllFileNamesAndPathsInDirectory", request, JsonOptions.JsonIgnore);
            return await result.Content.ReadFromJsonAsync<Dictionary<string, string>>() ?? throw new InvalidOperationException();
        }
        public async Task<byte[]> GetFirstOrDefaultFileInBytArrayAsync(GetFileRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Files/GetFirstOrDefaultFileInBytArray", request, JsonOptions.JsonIgnore);
            return await result.Content.ReadFromJsonAsync<byte[]>() ?? throw new InvalidOperationException();
        }
        public async Task<byte[]> GetFileInBytArrayAsync(GetFileRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Files/GetFileInBytArray", request, JsonOptions.JsonIgnore);
            return await result.Content.ReadFromJsonAsync<byte[]>() ?? throw new InvalidOperationException();
          
        }


        public async Task<GetManyFilesResponse> GetManyFileInBytArrayAsync(GetManyFileRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Files/GetManyFileInBytArray/{request}", request, JsonOptions.JsonIgnore);
            return await result.Content.ReadFromJsonAsync<GetManyFilesResponse>() ?? throw new InvalidOperationException();
        }
        public async Task<string> SaveFileAsync(SaveFileRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Files/SaveFile/{request}", request, JsonOptions.JsonIgnore);
            var tyy = await result.Content.ReadFromJsonAsync<SaveFileResponse>();
            return tyy.FileName ?? string.Empty;
        }
        public async Task<bool> RemoveFilesAsync(RemoveFileRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Files/RemoveFile", request, JsonOptions.JsonIgnore);
            return await result.Content.ReadFromJsonAsync<bool>();
        }

    }
}
