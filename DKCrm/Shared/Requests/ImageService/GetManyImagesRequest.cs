using DKCrm.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Requests.ImageService
{
    public class GetManyImagesRequest
    {
        public Guid OwnerId { get; set; }
        public DirectoryType DirectoryType { get; set; }
        public ImageTypes ImageType { get; set; }
        public bool IsPreviewImage { get; set; }
        public int FileCountSkip { get; set; }
        public int FileCountTake { get; set; }
        public bool IsFullPath { get; set; }
    }
}
