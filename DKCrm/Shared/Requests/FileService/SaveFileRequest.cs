using DKCrm.Shared.Constants;

namespace DKCrm.Shared.Requests.FileService
{
    public class SaveFileRequest
    {
        public string FileName { get; set; } = null!;
        public DirectoryType DirectoryType { get; set; }
        public string PathToDirectory { get; set; } = null!;
        public FileTypes? ContentType { get; set; }
        public byte[] Content { get; set; } = null!;
        public byte[]? Preview { get; set; }
        public bool IsFullPath { get; set; }
        public Guid OwnerId { get; set; }

    }
}
