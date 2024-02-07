using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ExportedOrder
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; }
        public string? Images { get; set; }
        public double? CurrencyPercent { get; set; }
        public string? TransactionCurrency { get; set; }
        public string? BuyerCurrency { get; set; }
        public string? LocalCurrency { get; set; }
        public virtual ICollection<ExportedProduct>? ExportedProducts { get; set; }
        public virtual ICollection<CommentOnExportedOrder>? Comments { get; set; }

        public virtual Company? OurCompany { get; set; }
        //[ForeignKey(nameof(OurCompany)), Column(Order = 0)] public Guid? OurCompanyId { get; set; }
        //public virtual Company? CompanyBuyer { get; set; }
        //[ForeignKey(nameof(CompanyBuyer)), Column(Order = 1)] public Guid? CompanyBuyerId { get; set; }

        //public virtual Employee? OurEmployee { get; set; }
        //[ForeignKey(nameof(OurEmployee)), Column(Order = 2)] public Guid? OurEmployeeId { get; set; }
        //public virtual Employee? EmployeeBuyer { get; set; }
        //[ForeignKey(nameof(EmployeeBuyer)), Column(Order = 3)] public Guid? EmployeeBuyerId { get; set; }
        public Guid? OurCompanyId { get; set; }
        public virtual Company? CompanyBuyer { get; set; }
        public Guid? CompanyBuyerId { get; set; }

        public virtual Employee? OurEmployee { get; set; }
        public Guid? OurEmployeeId { get; set; }
        public virtual Employee? EmployeeBuyer { get; set; }
        public Guid? EmployeeBuyerId { get; set; }

        public virtual ExportedOrderStatus ? ExportedOrderStatus { get; set; }
        public Guid? ExportedOrderStatusId { get; set; }
        public virtual ApplicationOrderingProducts? ApplicationOrderingProducts { get; set; }
        

    }
}
