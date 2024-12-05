using DKCrm.Server.Interfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Requests.FileService;

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
            var mainPath = directoryType == DirectoryType.PublicFolder ? _configuration[$"{DirectoryType.PublicFolder}"] 
                : _configuration[$"{DirectoryType.PrivateFolder}"];
            if (string.IsNullOrEmpty(mainPath))
                throw new NullReferenceException();
            
            return Path.Combine(mainPath,path);
        }
        public IEnumerable<GetFileInfoResponse> GetAllFileInfoInDirectory(GetFileRequest request)
        {
            var path = request.IsFullPath ? request.Path : BuildingPath(request.Path, request.DirectoryType);
            var dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
                return Array.Empty<GetFileInfoResponse>();
            var allFileInfo = dirInfo.GetFiles();
            var result = allFileInfo.Select(s=> 
                new GetFileInfoResponse()
                {
                    Path = s.FullName, FileName = s.Name, DateTimeCreate = s.CreationTimeUtc
                }
            );

            return result;
        }
        //public Dictionary<string,string> GetAllFileNamesAndPathsInDirectory(GetFileRequest request)
        //{
        //    var path = BuildingPath(request.Path, request.DirectoryType);
        //    var allFileInfo = new DirectoryInfo(path).GetFiles();

        //    return allFileInfo.ToDictionary(info => info.Name, info => info.FullName);
        //}
        public async Task<byte[]> GetFirstOrDefaultFileInBytArrayAsync(GetFileRequest request)
        {
            try
            {
                var path = request.IsFullPath ? request.Path : BuildingPath(request.Path, request.DirectoryType);
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
            var path = request.IsFullPath ? request.Path : BuildingPath(request.Path, request.DirectoryType);
            return await File.ReadAllBytesAsync(path);
        }
        public async Task<GetManyFilesResponse> GetManyFileInBytArrayAsync(GetManyFileRequest request)
        {
            var result = new GetManyFilesResponse
            {
                FileDictionary = new Dictionary<string, byte[]>()
            };
            var directoryItems = GetAllFileInfoInDirectory(new GetFileRequest()
            {
                Path = request.PathToDirectory,
                DirectoryType = request.DirectoryType, IsFullPath = request.IsFullPath
            }).ToArray();
            result.FileInDirectoryCount = directoryItems.Length;
            if (!directoryItems.Any() || directoryItems.Length == request.FileCountSkip) return new GetManyFilesResponse();
            directoryItems = directoryItems.Skip(request.FileCountSkip).Take(request.FileCountTake).ToArray();
            
            foreach (var item in directoryItems)
            {
               
                var byt = await File.ReadAllBytesAsync(item.Path);
                result.FileDictionary.Add(item.FileName,byt);
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
            var path = request.IsFullPath ? request.PathToDirectory : BuildingPath(request.PathToDirectory, request.DirectoryType);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var name = trustedFileName + $".{extension}";
            var fullPath = Path.Combine(path, name);
            await File.WriteAllBytesAsync(fullPath, request.Content);

            if (request.Preview != null)
            {
                var prevPath = Path.Combine(path,PathsToDirectories.Preview);
                if (!Directory.Exists(prevPath))
                    Directory.CreateDirectory(prevPath);
                var fullPrevPath = Path.Combine(prevPath, name);
                await File.WriteAllBytesAsync(fullPrevPath, request.Preview);
            }
            return new SaveFileResponse() { FileName = name, Path = path};
        }
        public bool RemoveFile(RemoveFileRequest request)
        {
            var path = request.IsFullPath ? request.Path : BuildingPath(request.Path, request.DirectoryType);
            var fullPath = Path.Combine(path);
            var fileExist = File.Exists(fullPath);
            if (fileExist)
                File.Delete(fullPath);
            return fileExist;
        }
    }
}
