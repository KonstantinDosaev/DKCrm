using DKCrm.Shared.Models.Products;

namespace DKCrm.Shared.Models.OrderModels
{
    public class SoldFromStorage 
    {
        public int Quantity { get; set; }
        public Guid ExportedProductId { get; set; }
        public virtual ExportedProduct? ExportedProduct { get; set; }
        public Guid StorageId { get; set; }
        public virtual Storage? Storage { get; set; }
    }
}
