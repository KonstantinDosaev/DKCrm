using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Shared.Models.Products
{
    public class Storage : IIdentifiable
    {
        public Guid Id { get; set; }
        //[Required]
        public string Name { get; set; }

        //[Required]
        public virtual Address? Address { get; set; }
        public Guid AddressId { get; set; }

        public virtual List<Product>? Products { get; set; }
        public virtual List<ProductsInStorage>? ProductsInStorage { get; set; }

        public virtual List<ExportedProduct>? ExportedProducts { get; set; }
        public virtual List<SoldFromStorage>? SoldFromStorageList { get; set; }

        public virtual List<ImportedProduct>? ImportedProducts { get; set; }
        public virtual List<PurchaseAtStorage>? PurchaseAtStorageList { get; set; }

    }
}
