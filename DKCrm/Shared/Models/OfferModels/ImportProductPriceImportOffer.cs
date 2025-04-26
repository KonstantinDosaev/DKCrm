using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Shared.Models.OfferModels
{
    public class ImportProductPriceImportOffer
    {
        public double Quantity { get; set; }
        public virtual ImportedProduct? ImportedProduct { get; set; }
        public Guid? ImportedProductId { get; set; }
        public Guid PriceId { get; set; }
        public virtual PriceForImportOffer? Price { get; set; }
        public DateTime DateTimeCreate { get; set; }
        public bool IsBlocked { get; set; }
    }
}
