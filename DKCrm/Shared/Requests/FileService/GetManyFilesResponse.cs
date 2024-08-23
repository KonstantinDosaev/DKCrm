using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Requests.FileService
{
    public class GetManyFilesResponse
    {
        public Dictionary<string, byte[]> FileDictionary { get; set; } = null!;
        public int FileInDirectoryCount { get; set; }
    }
}
