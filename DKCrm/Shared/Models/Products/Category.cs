using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.Products
{
    public class Category:IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public string? Description { get; set; }

        public Guid? ParentId { get; set; }
        public virtual Category? Parent { get; set; }

        public virtual ICollection<Category>? Children { get; set; }

        public virtual ICollection<Product>? Products { get; set; } 
        public virtual ICollection<CategoryOption>? CategoryOptions { get; set; }
        //public Guid? GrandCategoryId { get; set; }
        //public virtual GrandCategory? GrandCategory { get; set; }
 
    }
}
