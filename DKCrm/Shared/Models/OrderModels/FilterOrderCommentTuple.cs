namespace DKCrm.Shared.Models.OrderModels
{
    public class FilterOrderCommentTuple
    {
        public Guid OrderId { get; set; }
        public string? CreatedUserId { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? CreateDateFirst { get; set; }
        public DateTime? CreateDateLast { get; set; }
    }
}
