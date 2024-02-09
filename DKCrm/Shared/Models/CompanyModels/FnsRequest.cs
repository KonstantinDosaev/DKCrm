using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class FnsRequest: IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string INN { get; set; } = null!;
        public string ORGN { get; set; } = null!;
        public string Director { get; set; } = null!;
        public string OKVED { get; set; } = null!;
        public string? Revenue { get; set; }
        public string? LegalAddress { get; set; }

        public virtual Company? Company { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
