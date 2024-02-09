using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.OrderModels
{
    public class CommentOnImportedOrder : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = null!;

        public virtual ImportedOrder? ImportedOrder { get; set; }
        public Guid ImportedOrderId { get; set; }
    }
}
