using DKCrm.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Requests.ImageService
{
    public class SaveImageRequest
    {
        public string FileName { get; set; } = null!;
        public DirectoryType DirectoryType { get; set; }
        public byte[] Content { get; set; } = null!;
        public byte[]? Preview { get; set; }
        public Guid OwnerId { get; set; }
        public ImageTypes ImageType { get; set; }
        public bool IsFullPath { get; set; }
    }
}
