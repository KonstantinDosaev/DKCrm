using DKCrm.Server.Interfaces;
using DKCrm.Shared.Models;
using DKCrm.Shared.Requests.FileService;
using DKCrm.Shared.Requests.ImageService;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{infoSetId:guid}")]
        public async Task<IActionResult> GetOneInfoSet(Guid infoSetId)
            => Ok(await _imageService.GetOneInfoSetAsync(infoSetId));

        [HttpGet("{ownerId:guid}")]
        public async Task<IActionResult> GetAllInfoSetsToOwner(Guid ownerId)
            => Ok(await _imageService.GetAllInfoSetsToOwnerAsync(ownerId));

        [HttpGet("{infoSetId:guid}")]
        public async Task<IActionResult> GetOneImageInByteArr(Guid infoSetId)
            => Ok(await _imageService.GetOneImageInByteArrAsync(infoSetId));

        [HttpGet("{infoSetId:guid}")]
        public async Task<IActionResult> GetOnePreviewImageInByteArr(Guid infoSetId)
            => Ok(await _imageService.GetOnePreviewImageInByteArrAsync(infoSetId));

        [HttpPost("{request}")]
        public async Task<IActionResult> GetManyImageInBytArray(GetManyImagesRequest request)
            => Ok(await _imageService.GetManyImageInBytArrayAsync(request));

        [HttpPost]
        public async Task<IActionResult> AddImage(SaveImageRequest request)
            => Ok(await _imageService.AddImageAsync(request));

        [HttpPut]
        public async Task<IActionResult> UpdateInfoSetToImage(InfoSetToImage infoSet)
            => Ok(await _imageService.UpdateInfoSetToImageAsync(infoSet));

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveImage(Guid id)
            => Ok(await _imageService.RemoveImageAsync(id));
    }
}
