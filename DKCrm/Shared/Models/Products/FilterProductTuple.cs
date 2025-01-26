namespace DKCrm.Shared.Models.Products
{
    public class FilterProductTuple
    {
        public Guid? CategoryId { get; set; }
        public List<Guid>? BrandIdList { get; set; }
        public List<Guid>? ProductOptions { get; set; }
        public DateTime? CreateDateFirst { get; set; }
        public DateTime? CreateDateLast { get; set; }
        public DateTime? UpdateDateFirst { get; set; }
        public DateTime? UpdateDateLast { get; set; }
    }
}
