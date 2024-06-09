using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.OrderModels
{
    public class CommentOrder
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = null!;
        public string FromUserId { get; set; } = null!;
        public DateTime DateTimeCreated { get; set; }
        public Guid OrderId { get; set; }
    }
}
