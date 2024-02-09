using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class TagsRequest
    {
        public Guid CompanyId { get; set; }
        public List<Guid> TagCollection { get; set; } = new List<Guid>();
    }
}
