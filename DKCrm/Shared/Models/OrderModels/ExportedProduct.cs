﻿using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Models.Products;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.OfferModels;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ExportedProduct : ISoftDelete
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal? PriceLocal { get; set; }
        [MaxLength(10)]
        public string? TransactionCurrency { get; set; }
        public decimal? PriceInTransactionCurrency { get; set; }
        [MaxLength(10)]
        public string? BuyerCurrency { get; set; }
        public int MinDaysForDeliveryPlaned { get; set; }
        public int MaxDaysForDeliveryPlaned { get; set; }
        public decimal? PriceInBuyerCurrency { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        public DateTime? DateTimeConversionCurrency { get; set; }
        public virtual ICollection<ImportedProduct>? ImportedProducts { get; set; }
        public virtual ICollection<SoldFromStorage>? SoldFromStorage { get; set; }
        public virtual ICollection<PurchaseAtExport>? PurchaseAtExports { get; set; }
        public virtual ICollection<Storage>? StorageList { get; set; }
/*        public virtual ICollection<PriceForImportOffer>? PriceForImportOffers { get; set; }
        public virtual ICollection<ExportProductPriceImportOffer>? ExportProductPriceImportOffers { get; set; }*/

        public virtual Product? Product { get; set; }
        public Guid? ProductId { get; set; }
        public virtual ExportedOrder? ExportedOrder { get; set; }
        public Guid? ExportedOrderId { get; set; }


        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        [MaxLength(50)]
        public string? UpdatedUser { get; set; }
    }
}
