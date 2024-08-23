using DKCrm.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models
{
    public class ElementsBuildingsAPath
    { 
        public Guid? ParentId { get; set; }
        public Type? ObjectType { get; set; }
        public FileTypes FileType { get; set; }
    }
}
