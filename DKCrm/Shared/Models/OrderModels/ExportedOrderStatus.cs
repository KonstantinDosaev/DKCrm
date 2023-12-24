using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ExportedOrderStatus : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public int Position { get; set; }
        public virtual ICollection<ExportedOrder>? ExportedOrders { get; set; }
        public bool IsValueConstant { get; set; }
    }
}
