using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.OfferModels
{
    public class ExportProductPriceImportOffer
    {
        public double Quantity { get; set; }
        public virtual ImportedProduct? ImportedProduct { get; set; }
        public Guid? ImportedProductId { get; set; }
        public Guid ExportedProductsId { get; set; }
        public virtual ExportedProduct? ExportedProducts { get; set; }
        public Guid PriceId { get; set; }
        public virtual PriceForImportOffer? Price { get; set; }
        public DateTime DateTimeCreate { get; set; }

    }
}
