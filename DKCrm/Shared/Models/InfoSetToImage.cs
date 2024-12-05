using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Constants;

namespace DKCrm.Shared.Models
{
    public class InfoSetToImage
    {
        public Guid Id { get; set; }
        public int ImageType { get; set; }
        public Guid OwnerId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public DateTime DateTimeCreated { get; set; }
        [MaxLength(200)]
        public string PathToFile { get; set; } = null!;
        [MaxLength(200)]
        public string? PathToPreviewFile { get; set; }
        public  int DirectoryType { get; set; }

    }
}
