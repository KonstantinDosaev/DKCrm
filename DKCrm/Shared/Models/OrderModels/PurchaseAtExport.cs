using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.OrderModels
{
    public class PurchaseAtExport: IIdentifiable
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public Guid ExportedProductId { get; set; }
        public virtual ExportedProduct? ExportedProduct { get; set; }

        public Guid ImportedProductId { get; set; }
        public virtual ImportedProduct? ImportedProduct { get; set; }
    }
}
