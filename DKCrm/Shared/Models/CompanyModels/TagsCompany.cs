using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class TagsCompany : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Company>? Companies { get; set; }
    }
}
