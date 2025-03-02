using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Shared.Models.Products
{
    public class ProductsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; } = null!;
        public string? PartNumber { get; set; }
        public decimal? Price { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; }
        public DateOnly? DateDelivery { get; set; }
        public int? DayToDelivery { get; set; }
        public string? BrandName { get; set; }
        public Guid? BrandId { get; set; }
        public string? CategoryName { get; set; }
        public Guid? CategoryId { get; set; }
        public ICollection<ProductOption>? ProductOption { get; set; }
        public ICollection<Storage>? Storage { get; set; }

        public ICollection<ProductsInStorage>? ProductsInStorage { get; set; }

        public ICollection<ExportedProduct>? ExportedProducts { get; set; }
        public ICollection<ImportedProduct>? ImportedProducts { get; set; }
        public bool IsDeleted { get; set; }
    }
}
