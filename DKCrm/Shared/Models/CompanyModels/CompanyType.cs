using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class CompanyType : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Company>? Companies { get; set; }
    }
}
