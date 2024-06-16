using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.Products
{
    public class CategoryOption:IIdentifiable
    {
        public Guid Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; } = null!;
        [MaxLength(10)]
        public string? Measure { get; set; }
        public virtual ICollection<ProductOption>? ProductOption { get; set; }
        public virtual Category? Category { get; set; }
        public Guid CategoryId { get; set; }
    }
}
