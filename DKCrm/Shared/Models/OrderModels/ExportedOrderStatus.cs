using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ExportedOrderStatus : IIdentifiable, ISoftDelete
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = null!;
        public int Position { get; set; }
        public virtual ICollection<ExportedOrder>? ExportedOrders { get; set; }
        public virtual ICollection<ExportedOrderStatusExportedOrder>? ExportedOrderStatusExported { get; set; }
        public bool IsValueConstant { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string? UpdatedUser { get; set; }
    }
}
