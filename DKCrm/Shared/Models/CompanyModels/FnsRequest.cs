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
        public string Name { get; set; }
        public string INN { get; set; }
        public string ORGN { get; set; }
        public string Director { get; set; }
        public string OKVED { get; set; }
        public string? Revenue { get; set; }
        public string? LegalAddress { get; set; }

        public virtual Company? Company { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
