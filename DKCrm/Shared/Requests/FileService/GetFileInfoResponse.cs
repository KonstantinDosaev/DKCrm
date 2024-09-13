using DKCrm.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Requests.FileService
{
    public class GetFileInfoResponse
    {
        public string FileName { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTime DateTimeCreate { get; set; }
       
    }
}
