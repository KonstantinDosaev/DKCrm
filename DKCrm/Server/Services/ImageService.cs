using DKCrm.Server.Data;
using DKCrm.Server.Interfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Requests.FileService;
using DKCrm.Shared.Requests.ImageService;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDBContext _context;
        private readonly IFileService _fileService;
        public ImageService(ApplicationDBContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<InfoSetToImage> GetOneInfoSetAsync(Guid infoSetId)
        {
            return await _context.InfoSetsToImages.FirstOrDefaultAsync(w => w.Id == infoSetId) ?? throw new InvalidOperationException();
        }
        public async Task<IEnumerable<InfoSetToImage>> GetAllInfoSetsToOwnerAsync(Guid ownerId)
        {
            return await _context.InfoSetsToImages.Where(w => w.OwnerId == ownerId).ToArrayAsync();
        }
        public async Task<byte[]> GetOneImageInByteArrAsync(Guid infoSetId)
        {
            var infoSet = await GetOneInfoSetAsync(infoSetId);
            var byt = await _fileService.GetFileInBytArrayAsync(new GetFileRequest()
            {
                IsFullPath = false,
                DirectoryType = (DirectoryType)infoSet.DirectoryType,
                Path = infoSet.PathToFile
            });
            return byt;
        }

        public async Task<byte[]> GetOnePreviewImageInByteArrAsync(Guid infoSetId)
        {
            var infoSet = await GetOneInfoSetAsync(infoSetId);
            if (infoSet.PathToPreviewFile == null)
                return Array.Empty<byte>();
            var byt = await _fileService.GetFileInBytArrayAsync(new GetFileRequest()
            {
                IsFullPath = false,
                DirectoryType = (DirectoryType)infoSet.DirectoryType,
                Path = infoSet.PathToPreviewFile
            });
            return byt;
        }
        public async Task<GetManyImagesResponse> GetManyImageInBytArrayAsync(GetManyImagesRequest request)
        {
            var result = new GetManyImagesResponse()
            {
                FileDictionaryInfIdBytArr = new Dictionary<string, byte[]>()
            };
            var ownerImages = await GetAllInfoSetsToOwnerAsync(request.OwnerId);
            var filteredImages = ownerImages.Where(w => w.DirectoryType == (int)request.DirectoryType
                                                        && w.ImageType == (int)request.ImageType).ToArray();

           
            result.FileInDirectoryCount = request.IsPreviewImage ? filteredImages.Select(s => s.PathToPreviewFile).Count()
                : filteredImages.Select(s => s.PathToFile).Count();
            if (!filteredImages.Any() || filteredImages.Length == request.FileCountSkip) return new GetManyImagesResponse();
            filteredImages = filteredImages.Skip(request.FileCountSkip).Take(request.FileCountTake).ToArray();

            foreach (var item in filteredImages)
            {
                if (request.IsPreviewImage)
                {
                    if (item.PathToPreviewFile != null)
                    {
                        var byt = await File.ReadAllBytesAsync(item.PathToPreviewFile);
                        result.FileDictionaryInfIdBytArr.Add(item.Id.ToString(), byt);
                    }
                }
                else{
                    var byt = await File.ReadAllBytesAsync(item.PathToFile);
                    result.FileDictionaryInfIdBytArr.Add(item.Id.ToString(), byt);
                }
            }
            return result;
        }
        public async Task<int> AddImageAsync(SaveImageRequest request)
        {
            var pathToDirectory = Path.Combine(PathsToDirectories.FileContainer, PathsToDirectories.Images,
                request.ImageType.ToString(), request.OwnerId.ToString());
            var tt = await _fileService.SaveFileAsync(new SaveFileRequest()
            {
                DirectoryType = request.DirectoryType,
                FileName = request.FileName,
                IsFullPath = request.IsFullPath, 
                OwnerId = request.OwnerId, 
                ContentType = FileTypes.Images, 
                PathToDirectory = pathToDirectory,
                Content = request.Content,
                Preview = request.Preview
            });
            var result = 0;
            if (!string.IsNullOrEmpty(tt.FileName))
            {
                var infoSet = new InfoSetToImage()
                {
                    PathToFile = Path.Combine(pathToDirectory, tt.FileName), 
                    DateTimeCreated = DateTime.Now, 
                    DirectoryType = (int)request.DirectoryType,
                    PathToPreviewFile = Path.Combine(pathToDirectory,PathsToDirectories.Preview, tt.FileName),
                    OwnerId = request.OwnerId,
                    Name = request.FileName, ImageType = (int)request.ImageType
                };
                _context.InfoSetsToImages.Entry(infoSet).State = EntityState.Added;
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<int> UpdateInfoSetToImageAsync(InfoSetToImage infoSet)
        {
            _context.InfoSetsToImages.Entry(infoSet).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        public async Task<int> RemoveImageAsync(Guid id)
        {
            var infoSet = await GetOneInfoSetAsync(id);
        
            _fileService.RemoveFile(new RemoveFileRequest()
                {
                    FileType = FileTypes.Images,
                    DirectoryType = (DirectoryType)infoSet.DirectoryType,
                    Path = infoSet.PathToFile,
                    IsFullPath = false,
                    FileName = infoSet.Name
                });
            
            if (!string.IsNullOrEmpty(infoSet.PathToPreviewFile))
            {
                _fileService.RemoveFile(new RemoveFileRequest()
                    {
                        FileType = FileTypes.Images,
                        DirectoryType = (DirectoryType)infoSet.DirectoryType,
                        Path = infoSet.PathToPreviewFile,
                        IsFullPath = false,
                        FileName = infoSet.Name
                });
            }
            _context.InfoSetsToImages.Entry(infoSet).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }
    }
}
