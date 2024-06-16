using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.Products
{
    public class Category:IIdentifiable
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [MaxLength(200)]
        public string? Image { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        public virtual Category? Parent { get; set; }
        public virtual ICollection<Category>? Children { get; set; }
        public virtual ICollection<Product>? Products { get; set; } 
        public virtual ICollection<CategoryOption>? CategoryOptions { get; set; }
    }
}
