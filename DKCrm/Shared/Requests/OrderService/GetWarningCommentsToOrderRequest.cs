using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Requests
{
    public class GetWarningCommentsToOrderRequest
    {
        public bool GetOnlyNotOpen { get; set; }
        public string? OrderType { get; set; }
    }
}
