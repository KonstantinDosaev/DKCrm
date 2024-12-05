using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Requests.ImageService
{
    public class GetManyImagesResponse
    {
        public Dictionary<string, byte[]> FileDictionaryInfIdBytArr { get; set; } = null!;
        public int FileInDirectoryCount { get; set; }
    }
}
