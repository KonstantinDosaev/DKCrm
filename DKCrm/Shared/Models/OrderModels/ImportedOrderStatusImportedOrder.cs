using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ImportedOrderStatusImportedOrder : ISoftDelete
    {
        public Guid ImportedOrderId { get; set; }
        public virtual ImportedOrder? ImportedOrder { get; set; }

        public Guid ImportedOrderStatusId { get; set; }
        public virtual ImportedOrderStatus? ImportedOrderStatus { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeCreate { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string? UpdatedUser { get; set; }
    }
}
