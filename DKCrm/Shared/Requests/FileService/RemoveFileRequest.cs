using DKCrm.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Requests.FileService
{
    public class RemoveFileRequest
    {
        public string FileName { get; set; } = null!;
        public DirectoryType DirectoryType { get; set; }
        public FileTypes FileType { get; set; }
        public string Path { get; set; } = null!;
    }
}
