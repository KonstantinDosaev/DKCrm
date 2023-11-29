using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.Products;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class Company: IIdentifiable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Director { get; set; }
        public virtual Address? ActualAddress { get; set; }

        public virtual CompanyType? CompanyType { get; set; }
        public Guid? CompanyTypeId { get; set; }
        public virtual FnsRequest? FnsRequest { get; set; }
        public Guid? FnsRequestId { get; set; }
        public virtual ICollection<Employee>? Employees { get; set; }
        public virtual ICollection<BankDetails>? BankDetails { get; set; }
        public virtual ICollection<TagsCompany>? TagsCompanies { get; set; }

    }
}
