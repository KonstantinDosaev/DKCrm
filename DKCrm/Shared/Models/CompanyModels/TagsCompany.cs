using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class TagsCompany : IIdentifiable
    {
        public Guid Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; } = null!;
        public virtual ICollection<Company>? Companies { get; set; }
    }
}
