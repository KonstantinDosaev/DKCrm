using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.Products
{
    public class Product: IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; } = null!;
        public string? Description { get; set; }
        public string? PartNumber { get; set; }
        public decimal? Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int? Quantity { get; set; }
        public int? QuantityTwo { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; }
        public DateOnly? DateDelivery { get; set; }
        public int? DayToDelivery { get; set; }
        public virtual Brand? Brand { get; set; }
        public Guid? BrandId { get; set; }
        public virtual Category? Category { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual ICollection<ProductOption>? ProductOption { get; set; } 

    }
}
