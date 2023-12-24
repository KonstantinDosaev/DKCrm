using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ImportedOrderStatus : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public int Position { get; set; }
        public virtual ICollection<ImportedOrder>? ImportedOrders { get; set; }
        public bool IsValueConstant { get; set; }
    }
}
