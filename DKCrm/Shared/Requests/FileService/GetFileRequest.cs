using DKCrm.Shared.Constants;

namespace DKCrm.Shared.Requests.FileService
{
    public class GetFileRequest
    {
        public string Path { get; set; } = null!;
        public DirectoryType DirectoryType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
