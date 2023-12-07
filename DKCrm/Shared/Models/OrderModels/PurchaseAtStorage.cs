using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.Products;

namespace DKCrm.Shared.Models.OrderModels
{
    public class PurchaseAtStorage: IIdentifiable
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public Guid StorageId { get; set; }
        public virtual Storage? Storage { get; set; }

        public Guid ImportedProductId { get; set; }
        public virtual ImportedProduct? ImportedProduct { get; set; }
    }
}
