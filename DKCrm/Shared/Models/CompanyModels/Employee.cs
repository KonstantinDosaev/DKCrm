using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class Employee : IIdentifiable
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set;}
        public string? Description { get; set; }

        public virtual Company? Company { get; set; }
        public Guid CompanyId { get; set; }

        //[InverseProperty(nameof(ExportedOrder.OurEmployee))] public virtual ICollection<ExportedOrder>? ExportedOrdersOur { get; set; }
        //[InverseProperty(nameof(ExportedOrder.EmployeeBuyer))] public virtual ICollection<ExportedOrder>? ExportedOrdersBuyer { get; set; }
        //[InverseProperty(nameof(ImportedOrder.OurEmployee))] public virtual ICollection<ImportedOrder>? ImportedOrdersOur { get; set; }
        //[InverseProperty(nameof(ImportedOrder.EmployeeSeller))] public virtual ICollection<ImportedOrder>? ImportedOrdersSellers { get; set; }
        public virtual ICollection<ExportedOrder>? ExportedOrdersOur { get; set; }
        public virtual ICollection<ExportedOrder>? ExportedOrdersBuyer { get; set; }
        public virtual ICollection<ImportedOrder>? ImportedOrdersOur { get; set; }
        public virtual ICollection<ImportedOrder>? ImportedOrdersSellers { get; set; }
    }
}
