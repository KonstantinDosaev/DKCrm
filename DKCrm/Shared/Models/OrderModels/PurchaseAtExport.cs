using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.OrderModels
{
    public class PurchaseAtExport
    {
        public int Quantity { get; set; }

        public Guid ExportedProductId { get; set; }
        public virtual ExportedProduct? ExportedProduct { get; set; }

        public Guid ImportedProductId { get; set; }
        public virtual ImportedProduct? ImportedProduct { get; set; }
    }
}
