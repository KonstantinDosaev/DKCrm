using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Requests
{
    public class DeleteForGuidRequest
    {
        public Guid Id { get; set; }
        public bool IsFullDelete { get; set; }
    }
}
