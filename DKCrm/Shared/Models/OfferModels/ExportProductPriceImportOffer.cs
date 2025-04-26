using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Shared.Models.OfferModels
{
    public class ExportProductPriceImportOffer
    {
        public double Quantity { get; set; }
/*        public virtual ImportedProduct? ImportedProduct { get; set; }
        public Guid? ImportedProductId { get; set; }*/
        public Guid ExportedProductId { get; set; } 
        public virtual ExportedProduct? ExportedProduct { get; set; }
        public Guid PriceId { get; set; }
        public virtual PriceForImportOffer? Price { get; set; }
        public DateTime DateTimeCreate { get; set; }
        public DateTime DateTimeUpdate { get; set; }
        public bool IsAddImport { get; set; }
        public bool IsBlocked { get; set; }

    }
}
