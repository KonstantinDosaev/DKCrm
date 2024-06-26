﻿using DKCrm.Shared.Models.Products;

namespace DKCrm.Shared.Models.OrderModels
{
    public class PurchaseAtStorage
    {
        public int Quantity { get; set; }
        public Guid StorageId { get; set; }
        public virtual Storage? Storage { get; set; }
        public Guid ImportedProductId { get; set; }
        public virtual ImportedProduct? ImportedProduct { get; set; }
    }
}
