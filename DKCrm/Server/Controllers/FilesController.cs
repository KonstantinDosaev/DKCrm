using DKCrm.Server.Interfaces;
using DKCrm.Shared.Requests.FileService;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public  IActionResult GetAllFileInfoInDirectory(GetFileRequest request)
            => Ok( _fileService.GetAllFileInfoInDirectory(request));
        //[HttpPost]
        //public IActionResult GetAllFileNamesAndPathsInDirectory(GetFileRequest request)
        //    => Ok(_fileService.GetAllFileNamesAndPathsInDirectory(request));

        [HttpPost]
        public async Task<IActionResult> GetFirstOrDefaultFileInBytArray(GetFileRequest request)
            => Ok(await _fileService.GetFirstOrDefaultFileInBytArrayAsync(request));

        [HttpPost]
        public async Task<IActionResult> GetFileInBytArray(GetFileRequest request)
            => Ok(await _fileService.GetFileInBytArrayAsync(request));

        [HttpPost("{request}")]
        public async Task<IActionResult> GetManyFileInBytArray(GetManyFileRequest request)
            => Ok(await _fileService.GetManyFileInBytArrayAsync(request));

        [HttpPost("{request}")]
        public async Task<IActionResult> SaveFile(SaveFileRequest request)
            => Ok(await _fileService.SaveFileAsync(request));

        [HttpPost]
        public IActionResult RemoveFile(RemoveFileRequest request)
            => Ok( _fileService.RemoveFile(request));




    }
}
