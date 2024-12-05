using DKCrm.Client.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Requests.FileService;
using DKCrm.Shared.Requests.ImageService;
using System.Net.Http;

namespace DKCrm.Client.Services.ImageService
{
    public interface IImageManager
    {
        Task<IEnumerable<InfoSetToImage>> GetAllInfoSetsToOwnerAsync(Guid ownerId);
        Task<InfoSetToImage> GetOneInfoSetAsync(Guid infoSetId);
        Task<byte[]> GetOneImageInByteArrAsync(Guid infoSetId);
        Task<byte[]> GetOnePreviewImageInByteArrAsync(Guid infoSetId);
        Task<GetManyImagesResponse> GetManyImageInBytArrayAsync(GetManyImagesRequest request);
        Task<bool> RemoveImageAsync(Guid infoSetId);
        Task<bool> AddImageAsync(SaveImageRequest request);
        Task<bool> UpdateInfoSetToImageAsync(InfoSetToImage infoSet);
    }
}
