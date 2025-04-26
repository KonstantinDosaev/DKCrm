using DKCrm.Shared.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;
using System.ComponentModel.DataAnnotations;

namespace DKCrm.Shared.Models.OfferModels
{
    public class PriceForImportOffer : IIdentifiable, ISoftDelete
    {
        public Guid Id { get; set; }
        public double Quantity { get; set; }
        public decimal Value { get; set; }
        [MaxLength(10)] public string Currency { get; set; } = null!;
        public virtual ImportOffer? ImportOffer { get; set; }
        public Guid? ImportOfferId { get; set; }
        public DateTime DateTimeCreate { get; set; }
        public DateTime? DateTimeOver { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        [MaxLength(50)]
        public string? UpdatedUser { get; set; }

        public decimal ValueLocal { get; set; }
        [MaxLength(10)] public string CurrencyLocal { get; set; } = null!;
        public double CurrencyPercent { get; set; }
        public double Nds { get; set; }

        public virtual ICollection<ExportedProduct>? ExportedProducts { get; set; }
        public virtual ICollection<ImportedProduct>? ImportedProducts { get; set; }
        public virtual ICollection<ExportProductPriceImportOffer>? ExportProductPriceImportOffers { get; set; }
        public virtual ICollection<ImportProductPriceImportOffer>? ImportProductPriceImportOffers { get; set; }

    }
}
