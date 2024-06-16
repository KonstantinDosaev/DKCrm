using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.Products;
namespace DKCrm.Shared.Models.OrderModels
{
    public class ApplicationOrderingProducts: ISoftDelete
    {
        public Guid Id { get; set; }
        [MaxLength(20)]
        public string Number { get; set; } = null!;
        [MaxLength(50)]
        public string? UserId { get; set; }
        [MaxLength(50)]
        public string? UserName { get; set; }
        [MaxLength(50)]
        public string? CompanyName { get; set; }
        [MaxLength(50)]
        public string? CompanyInn { get; set; }
        public virtual ICollection<Product>? ProductList { get; set; }
        [MaxLength(20)]
        public string? Email { get; set; }
        [MaxLength(20)]
        public string? Phone { get; set; }
        public virtual ICollection<ApplicationOrderingProductsProduct>? ApplicationOrderingProductProduct { get; set; }
        public bool? InWork { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        [MaxLength(50)]
        public string? UpdatedUser { get; set; }
        [MaxLength(50)]
        public string? TakerUser { get; set; }
        [MaxLength(50)]
        public string? TakerUserId { get; set; }
        public DateTime? DateTimeTake{ get; set; }
        [MaxLength(2400)]
        public string? MissingProductsInCatalog { get; set; }

        public virtual ExportedOrder? ExportedOrder { get; set; }
        public Guid? ExportedOrderId { get; set; }
    }
}
