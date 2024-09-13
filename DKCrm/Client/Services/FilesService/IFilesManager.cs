using DKCrm.Shared.Requests.FileService;

namespace DKCrm.Client.Services.FilesService
{
    public interface IFilesManager
    {
        Task<IEnumerable<GetFileInfoResponse>> GetAllFileInfoInDirectoryAsync(GetFileRequest request);
        //Task<Dictionary<string, string>>GetAllFileNamesAndPathsInDirectoryAsync(GetFileRequest request);
        Task<string> SaveFileAsync(SaveFileRequest request);
        Task<byte[]> GetFirstOrDefaultFileInBytArrayAsync(GetFileRequest request);
        Task<byte[]> GetFileInBytArrayAsync(GetFileRequest request);
        Task<GetManyFilesResponse> GetManyFileInBytArrayAsync(GetManyFileRequest request);
        Task<bool> RemoveFilesAsync(RemoveFileRequest request);
    }
}
