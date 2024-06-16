namespace DKCrm.Shared.Models.CompanyModels
{
    public class TagsRequest
    {
        public Guid CompanyId { get; set; }
        public List<Guid> TagCollection { get; set; } = new List<Guid>();
    }
}
