using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.Products;
namespace DKCrm.Shared.Models.OrderModels
{
    public class ApplicationOrderingProducts: ISoftDelete
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyInn { get; set; }
        public virtual ICollection<Product>? ProductList { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public virtual ICollection<ApplicationOrderingProductsProduct>? ApplicationOrderingProductProduct { get; set; }
        public bool? InWork { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string? UpdatedUser { get; set; }

        public string? MissingProductsInCatalog { get; set; }

        public virtual ExportedOrder? ExportedOrder { get; set; }
        public Guid? ExportedOrderId { get; set; }
    }
}
