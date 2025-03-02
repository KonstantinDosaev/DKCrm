using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.UserAuth
{
    public class UserEmailSettings
    {
        public Guid Id { get; set; }
        public string MailServer { get; set; } = null!;
        public string MailPort { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public string Sender { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public virtual ApplicationUser? User { get; set; }
    }
}
