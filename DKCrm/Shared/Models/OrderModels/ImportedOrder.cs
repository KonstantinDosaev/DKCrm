using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ImportedOrder : IIdentifiable
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; }
        public string? Images { get; set; }
        public double? CurrencyPercent { get; set; }
        public string? TransactionCurrency { get; set; }
        public string? SupplierCurrency { get; set; }
        public string? LocalCurrency { get; set; }

        public virtual ICollection<ImportedProduct>? ImportedProducts { get; set; }
        public virtual ICollection<CommentOnImportedOrder>? Comments { get; set; }

        //public virtual Company? OurCompany { get; set; }
        //[ForeignKey(nameof(OurCompany)), Column(Order = 0)] public Guid? OurCompanyId { get; set; }
        //public virtual Company? SellersCompany { get; set; }
        //[ForeignKey(nameof(SellersCompany)), Column(Order = 1)] public Guid? SellersCompanyId { get; set; }

        //public virtual Employee? OurEmployee { get; set; }
        //[ForeignKey(nameof(OurEmployee)), Column(Order = 2)] public Guid? OurEmployeeId { get; set; }
        //public virtual Employee? EmployeeSeller{ get; set; }
        //[ForeignKey(nameof(EmployeeSeller)), Column(Order = 3)] public Guid? EmployeeSellerId { get; set; }
        public virtual Company? OurCompany { get; set; }
        public Guid? OurCompanyId { get; set; }
        public virtual Company? SellersCompany { get; set; }
        public Guid? SellersCompanyId { get; set; }

        public virtual Employee? OurEmployee { get; set; }
        public Guid? OurEmployeeId { get; set; }
        public virtual Employee? EmployeeSeller { get; set; }
        public Guid? EmployeeSellerId { get; set; }


        public virtual ImportedOrderStatus? ImportedOrderStatus { get; set; }
        public Guid? ImportedOrderStatusId { get; set; }
    }
}
