using DKCrm.Server.Interfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Requests.FileService;
using OpenXmlPowerTools;
using Org.BouncyCastle.Utilities;
using System.Drawing.Imaging;
using System.Drawing;
using DocumentFormat.OpenXml.Office2013.Excel;
using System.Security.Claims;
using Microsoft.Extensions.FileProviders;

namespace DKCrm.Server.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        
        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string BuildingPath(string path, DirectoryType directoryType)
        {
            if (path[0] != '\\') return path;
            
            var mainPath = directoryType == DirectoryType.Public ? _configuration["PathToPublicDirectory"] 
                : _configuration["PathToStaticFiles"];

            mainPath += path;
            return mainPath;
        }
        public IEnumerable<string> GetAllFileNamesInDirectory(GetFileRequest request, ClaimsPrincipal user)
        {
            var path = BuildingPath(request.Path, request.DirectoryType);
            var allFileInfo = new DirectoryInfo(path).GetFiles().Select(s=>s.Name);

            return allFileInfo;
        }
        public Dictionary<string,string> GetAllFileNamesAndPathsInDirectory(GetFileRequest request)
        {
            var path = BuildingPath(request.Path, request.DirectoryType);
            var allFileInfo = new DirectoryInfo(path).GetFiles();

            return allFileInfo.ToDictionary(info => info.Name, info => info.FullName);
        }
        public async Task<byte[]> GetFirstOrDefaultFileInBytArrayAsync(GetFileRequest request)
        {
            try
            {
                var path = BuildingPath(request.Path, request.DirectoryType);
                var allFileInfo = new DirectoryInfo(path).GetFiles();
                if (!allFileInfo.Any()) return Array.Empty<byte>();
                var defaultFile = allFileInfo.Where(w => w.Name.Contains("default")).Select(s => s.FullName).FirstOrDefault();
                var fullPath = defaultFile ?? allFileInfo.Select(s => s.FullName).FirstOrDefault();
                var bytes = await File.ReadAllBytesAsync(fullPath!);
                return bytes;
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        public async Task<byte[]> GetFileInBytArrayAsync(GetFileRequest request)
        {
            var path = BuildingPath(request.Path, request.DirectoryType);
            return await File.ReadAllBytesAsync(path);
        }
        public async Task<GetManyFilesResponse> GetManyFileInBytArrayAsync(GetManyFileRequest request)
        {
            var result = new GetManyFilesResponse
            {
                FileDictionary = new Dictionary<string, byte[]>()
            };
            var directoryItems = GetAllFileNamesAndPathsInDirectory(new GetFileRequest()
            {
                Path = request.PathToDirectory,
                DirectoryType = request.DirectoryType
            }).ToArray();
            result.FileInDirectoryCount = directoryItems.Length;
            if (!directoryItems.Any() || directoryItems.Length == request.FileCountSkip) return new GetManyFilesResponse();
            directoryItems = directoryItems.Skip(request.FileCountSkip).Take(request.FileCountTake).ToArray();
            
            foreach (var item in directoryItems)
            {
               
                var byt = await File.ReadAllBytesAsync(item.Value);
                result.FileDictionary.Add(item.Key,byt);
            }
            //var outDot = directoryItems.Length - request.FileCountSkip > request.FileCountTake ?
            //    request.FileCountTake : directoryItems.Length;
            //for (var i = request.FileCountSkip-1; i <= outDot; i++)
            //{
            //    result.Add(await File.ReadAllBytesAsync(path));
            //}
            return result ;
        }
        public async Task<SaveFileResponse> SaveFileAsync(SaveFileRequest request)
        {
            var extension = request.FileName.Split('.').Last();
            var trustedFileName = Path.GetRandomFileName();
            var path = BuildingPath(request.Path, request.DirectoryType);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var name = trustedFileName + $".{extension}";
            var fullPath = Path.Combine(path, name);
            await File.WriteAllBytesAsync(fullPath, request.Content);

            if (request.Preview != null)
            {
                var prevPath = path + PathsToDirectories.Preview;
                if (!Directory.Exists(prevPath))
                    Directory.CreateDirectory(prevPath);
                var fullPrevPath = Path.Combine(prevPath, name);
                await File.WriteAllBytesAsync(fullPrevPath, request.Preview);
            }
            return new SaveFileResponse() { FileName = name };
        }
        public bool RemoveFile(RemoveFileRequest request)
        {
           
            var path = BuildingPath(request.Path, request.DirectoryType);

            var fullPath = Path.Combine(path, request.FileName);
            var fileInfo = new FileInfo(fullPath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            else
                return false;

            if (request.FileType is FileTypes.Stamps or FileTypes.Images)
            {
                var prevPath = path + PathsToDirectories.Preview;
                var prevFullPath = Path.Combine(prevPath, request.FileName);
                var prevFileInfo = new FileInfo(prevFullPath);
                if (prevFileInfo.Exists)
                {
                    prevFileInfo.Delete();
                }
            }
            return !fileInfo.Exists;
        }
    }
}
