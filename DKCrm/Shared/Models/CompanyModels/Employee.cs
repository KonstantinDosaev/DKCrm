using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class Employee : IIdentifiable, ISoftDelete
    {
        public Guid Id { get; set; }
        [MaxLength(30)]
        public string? FirstName { get; set; }
        [MaxLength(30)]
        public string? MiddleName { get; set; }
        [MaxLength(30)]
        public string? LastName { get; set; }
        [MaxLength(30)]
        public string? Position { get; set; }
        [MaxLength(30)]
        public string? Phone { get; set; }
        [MaxLength(50)]
        public string? Email { get; set;}
        [MaxLength(200)]
        public string? Description { get; set; }
        public bool IsOurEmployee { get; set; }
      
        public virtual ICollection<Company>? Companies { get; set; }
        [MaxLength(50)]
        public string? UserId { get; set; }

        //[InverseProperty(nameof(ExportedOrder.OurEmployee))] public virtual ICollection<ExportedOrder>? ExportedOrdersOur { get; set; }
        //[InverseProperty(nameof(ExportedOrder.EmployeeBuyer))] public virtual ICollection<ExportedOrder>? ExportedOrdersBuyer { get; set; }
        //[InverseProperty(nameof(ImportedOrder.OurEmployee))] public virtual ICollection<ImportedOrder>? ImportedOrdersOur { get; set; }
        //[InverseProperty(nameof(ImportedOrder.EmployeeSeller))] public virtual ICollection<ImportedOrder>? ImportedOrdersSellers { get; set; }
        public virtual ICollection<ExportedOrder>? ExportedOrdersOur { get; set; }
        public virtual ICollection<ExportedOrder>? ExportedOrdersBuyer { get; set; }
        public virtual ICollection<ImportedOrder>? ImportedOrdersOur { get; set; }
        public virtual ICollection<ImportedOrder>? ImportedOrdersSellers { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string? UpdatedUser { get; set; }
    }
}
