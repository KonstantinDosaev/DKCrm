using DKCrm.Server.Services;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Requests.FileService;
using DKCrm.Shared.Requests.ImageService;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Interfaces
{
    public interface IImageService
    {
        Task<InfoSetToImage> GetOneInfoSetAsync(Guid infoSetId);
        Task<IEnumerable<InfoSetToImage>> GetAllInfoSetsToOwnerAsync(Guid ownerId);
        Task<byte[]> GetOneImageInByteArrAsync(Guid infoSetId);
        Task<byte[]> GetOnePreviewImageInByteArrAsync(Guid infoSetId);
        Task<GetManyImagesResponse> GetManyImageInBytArrayAsync(GetManyImagesRequest request);
        Task<int> AddImageAsync(SaveImageRequest request);
        Task<int> UpdateInfoSetToImageAsync(InfoSetToImage infoSet);
        Task<int> RemoveImageAsync(Guid id);
       
    }
}
