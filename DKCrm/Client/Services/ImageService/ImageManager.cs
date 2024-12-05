using DKCrm.Client.Constants;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using System.Net.Http.Json;
using DKCrm.Shared.Requests.FileService;
using DKCrm.Shared.Requests.ImageService;

namespace DKCrm.Client.Services.ImageService
{
    public class ImageManager : IImageManager
    {
        private readonly HttpClient _httpClient;

        public ImageManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<InfoSetToImage>> GetAllInfoSetsToOwnerAsync(Guid ownerId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<InfoSetToImage>>
                ($"api/Images/GetAllInfoSetsToOwner/{ownerId}") ?? throw new InvalidOperationException();
        }
        public async Task<InfoSetToImage> GetOneInfoSetAsync(Guid infoSetId)
        {
            return await _httpClient.GetFromJsonAsync<InfoSetToImage>
                ($"api/Images/GetOneInfoSet/{infoSetId}") ?? throw new InvalidOperationException();
        }
        public async Task<byte[]> GetOneImageInByteArrAsync(Guid infoSetId)
        {
            return await _httpClient.GetFromJsonAsync<byte[]>
                ($"api/Images/GetOneImageInByteArr/{infoSetId}") ?? throw new InvalidOperationException();
        }
        public async Task<byte[]> GetOnePreviewImageInByteArrAsync(Guid infoSetId)
        {
            return await _httpClient.GetFromJsonAsync<byte[]>
                ($"api/Images/GetOnePreviewImageInByteArr/{infoSetId}") ?? throw new InvalidOperationException();
        }
        public async Task<GetManyImagesResponse> GetManyImageInBytArrayAsync(GetManyImagesRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Images/GetManyImageInBytArray/{request}", request, JsonOptions.JsonIgnore);
            return await result.Content.ReadFromJsonAsync<GetManyImagesResponse>() ?? throw new InvalidOperationException();
        }
        public async Task<bool> RemoveImageAsync(Guid infoSetId)
        {
            var result = await _httpClient.DeleteAsync($"api/Images/RemoveImage/{infoSetId}");
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> AddImageAsync(SaveImageRequest request)
        {
            var result = await _httpClient
                .PostAsJsonAsync($"api/Images/AddImage", request, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateInfoSetToImageAsync(InfoSetToImage infoSet)
        {
            var result = await _httpClient
                .PutAsJsonAsync($"api/Images/UpdateInfoSetToImage",infoSet, JsonOptions.JsonIgnore);
            return result.IsSuccessStatusCode;
        }
       
       
       
       
    }
}
