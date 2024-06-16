using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ExportedOrder : ISoftDelete
    {
        public Guid Id { get; set; }
        public string? Number { get; set; }
        public double? Nds { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        [MaxLength(1000)]
        public string? Images { get; set; }
        public bool IsAllProductsAreCollected { get; set; }
        public bool OrderIsOver { get; set; }
        public double? CurrencyPercent { get; set; }
        [MaxLength(10)]
        public string? TransactionCurrency { get; set; }
        [MaxLength(10)]
        public string? BuyerCurrency { get; set; }
        [MaxLength(10)]
        public string? LocalCurrency { get; set; }
        public virtual ICollection<ExportedProduct>? ExportedProducts { get; set; }
        public virtual ICollection<ExportedOrderStatus>? ExportedOrderStatus { get; set; }
        public virtual ICollection<ExportedOrderStatusExportedOrder>? ExportedOrderStatusExported { get; set; }

        public virtual Company? OurCompany { get; set; }
        public Guid? OurCompanyId { get; set; }
        public virtual Company? CompanyBuyer { get; set; }
        public Guid? CompanyBuyerId { get; set; }

        public virtual Employee? OurEmployee { get; set; }
        public Guid? OurEmployeeId { get; set; }
        public virtual Employee? EmployeeBuyer { get; set; }
        public Guid? EmployeeBuyerId { get; set; }

        public virtual ApplicationOrderingProducts? ApplicationOrderingProducts { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        [MaxLength(50)]
        public string? UpdatedUser { get; set; }
    }
}
