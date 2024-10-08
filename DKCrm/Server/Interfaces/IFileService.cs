﻿using DKCrm.Shared.Requests.FileService;
using System.Security.Claims;

namespace DKCrm.Server.Interfaces
{
    public interface IFileService
    {
        IEnumerable<GetFileInfoResponse> GetAllFileInfoInDirectory(GetFileRequest request);
       //Dictionary<string, string> GetAllFileNamesAndPathsInDirectory(GetFileRequest request);
        Task<SaveFileResponse> SaveFileAsync(SaveFileRequest request);
        Task<byte[]> GetFirstOrDefaultFileInBytArrayAsync(GetFileRequest request);
        Task<byte[]> GetFileInBytArrayAsync(GetFileRequest request);
        Task<GetManyFilesResponse> GetManyFileInBytArrayAsync(GetManyFileRequest request);
        bool RemoveFile(RemoveFileRequest request);
    }
}
