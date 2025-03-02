using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Requests
{
    public class SendEmailRequest
    {
        public List<string> Emails { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
        public Dictionary<string, byte[]>? Attachments { get; set; }
        public bool SendByUser { get; set; }
    }
}
