﻿using DKCrm.Shared.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ExportedProduct
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal? PriceLocal { get; set; }
        public string? TransactionCurrency { get; set; }
        public decimal? PriceInTransactionCurrency { get; set; }
        public string? BuyerCurrency { get; set; }
        public decimal? PriceInBuyerCurrency { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<ImportedProduct>? ImportedProducts { get; set; }
        public virtual ICollection<SoldFromStorage>? SoldFromStorage { get; set; }
        public virtual ICollection<PurchaseAtExport>? PurchaseAtExports { get; set; }
        public virtual ICollection<Storage>? StorageList { get; set; }

        public virtual Product? Product { get; set; }
        public Guid? ProductId { get; set; }
        public virtual ExportedOrder? ExportedOrder { get; set; }
        public Guid? ExportedOrderId { get; set; }
      
      
    }
}