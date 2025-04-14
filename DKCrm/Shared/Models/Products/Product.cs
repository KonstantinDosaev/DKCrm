using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.OfferModels;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Shared.Models.Products
{
    public class Product: IIdentifiable, ISoftDelete
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [MaxLength(200)]
        public string? Image { get; set; } = null!;
        [MaxLength(200)]
        public string? Description { get; set; }
        [MaxLength(30)]
        public string? PartNumber { get; set; }
        public decimal? Price { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; }
        public DateOnly? DateDelivery { get; set; }
        public int? DayToDelivery { get; set; }
        public virtual Brand? Brand { get; set; }
        public Guid? BrandId { get; set; }
        public virtual Category? Category { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual ICollection<ProductOption>? ProductOption { get; set; }
        public virtual ICollection<Storage>? Storage { get; set; } 
        public virtual ICollection<ProductsInStorage>? ProductsInStorage { get; set; }
        public virtual ICollection<ExportedProduct>? ExportedProducts { get; set; }
        public virtual ICollection<ImportedProduct>? ImportedProducts { get; set; }
        public virtual ICollection<ApplicationOrderingProducts>? ApplicationOrderingList { get; set; }
        public virtual ICollection<ApplicationOrderingProductsProduct>? ApplicationOrderingProductProduct { get; set; }
        public virtual ICollection<ImportOffer>? ImportOffers { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        [MaxLength(50)]
        public string? UpdatedUser { get; set; }
    }
}
