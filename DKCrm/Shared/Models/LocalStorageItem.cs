using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models
{
    public class LocalStorageItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public byte[] Attachment { get; set; }
    }
}
