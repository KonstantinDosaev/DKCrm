using DKCrm.Shared.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.Chat
{
    public class LogUsersVisitToChat
    {
        public DateTime DateTimeVisit { get; set; }

        public string ApplicationUserId { get; set; } = null!;
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public Guid ChatGroupId { get; set; }
        public virtual ChatGroup? ChatGroup { get; set; }
    }
}
