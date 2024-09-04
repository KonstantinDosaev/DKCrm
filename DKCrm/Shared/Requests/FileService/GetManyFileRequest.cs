using DKCrm.Shared.Constants;

namespace DKCrm.Shared.Requests.FileService
{
    public class GetManyFileRequest
    {
        public string PathToDirectory { get; set; } = null!;
        public DirectoryType DirectoryType { get; set; }
        public FileTypes FileTypes { get; set; }
        public int FileCountSkip { get; set; }
        public int FileCountTake { get; set; }

    }
}
