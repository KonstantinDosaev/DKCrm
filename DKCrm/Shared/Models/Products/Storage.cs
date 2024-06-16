using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Shared.Models.Products
{
    public class Storage : IIdentifiable, ISoftDelete
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public virtual Address? Address { get; set; }
        public Guid? AddressId { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
         public virtual ICollection<ProductsInStorage>? ProductsInStorage { get; set; }
        public virtual ICollection<ExportedProduct>? ExportedProducts { get; set; }
        public virtual ICollection<SoldFromStorage>? SoldFromStorageList { get; set; }
        public virtual ICollection<ImportedProduct>? ImportedProducts { get; set; }
        public virtual ICollection<PurchaseAtStorage>? PurchaseAtStorageList { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        [MaxLength(50)]
        public string? UpdatedUser { get; set; }
    }
}
