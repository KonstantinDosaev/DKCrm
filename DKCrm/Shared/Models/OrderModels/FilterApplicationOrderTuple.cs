namespace DKCrm.Shared.Models.OrderModels
{
    public class FilterApplicationOrderTuple
    {
        public DateTime? CreateDateFirst { get; set; }
        public DateTime? CreateDateLast { get; set; }
        public DateTime? UpdateDateFirst { get; set; }
        public DateTime? UpdateDateLast { get; set; }
        public string? UserId { get; set; }
        public bool? InWork { get; set; }
    }
}
