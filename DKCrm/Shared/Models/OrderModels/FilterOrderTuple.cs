namespace DKCrm.Shared.Models.OrderModels
{
    public class FilterOrderTuple
    {
        public List<Guid>? OurCompanies { get; set; }
        public List<Guid>? ContragentsCompanies { get; set; }
        public bool IsHistoryOrders { get; set; }
        public bool? IsCompleteOrders { get; set; }
        public Guid? CurrentStatusId { get; set; }
        public Guid? CurrentPartnerCompanyId { get; set; }
        public Guid? CurrentOurCompanyId { get; set; }
        public Guid? CurrentOurEmployeeId { get; set; }
        public Guid? CurrentPartnerEmployeeId { get; set; }
        public DateTime? CreateDateFirst { get; set; }
        public DateTime? CreateDateLast { get; set; }
        public DateTime? UpdateDateFirst { get; set; }
        public DateTime? UpdateDateLast { get; set; }
        public bool IsOrdersWithUnreadComments { get; set; }
        public bool IsOrdersWithUnreadSystemComments { get; set; }

    }
}
