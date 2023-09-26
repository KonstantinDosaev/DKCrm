using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.Products
{
    public class Brand:IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = null!;
    }
}
