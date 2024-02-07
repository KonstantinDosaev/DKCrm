using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models.Products;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class Company: IIdentifiable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Director { get; set; }
        public string? Inn { get; set; }
        public virtual Address? ActualAddress { get; set; }
        public  Guid? ActualAddressId { get; set; }
        public virtual CompanyType? CompanyType { get; set; }
        public Guid? CompanyTypeId { get; set; }
        public virtual FnsRequest? FnsRequest { get; set; }
        public Guid? FnsRequestId { get; set; }
        public virtual ICollection<Employee>? Employees { get; set; }
        public virtual ICollection<BankDetails>? BankDetails { get; set; }
        public virtual ICollection<TagsCompany>? TagsCompanies { get; set; }

        //[InverseProperty(nameof(ExportedOrder.OurCompany))] public virtual ICollection<ExportedOrder>? ExportedOrdersOurCompany { get; set; }
        //[InverseProperty(nameof(ExportedOrder.CompanyBuyer))] public virtual ICollection<ExportedOrder>? ExportedOrdersBuyerCompany { get; set; }
        //[InverseProperty(nameof(ImportedOrder.OurCompany))] public virtual ICollection<ImportedOrder>? ImportedOrdersOurCompany { get; set; }
        //[InverseProperty(nameof(ImportedOrder.SellersCompany))] public virtual ICollection<ImportedOrder>? ImportedOrdersSellersCompany { get; set; }

        public virtual ICollection<ExportedOrder>? ExportedOrdersOurCompany { get; set; }
        public virtual ICollection<ExportedOrder>? ExportedOrdersBuyerCompany { get; set; }
        public virtual ICollection<ImportedOrder>? ImportedOrdersOurCompany { get; set; }
        public virtual ICollection<ImportedOrder>? ImportedOrdersSellersCompany { get; set; }
    }
}
