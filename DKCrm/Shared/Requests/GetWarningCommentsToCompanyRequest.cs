using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Requests
{
    public class GetWarningCommentsToCompanyRequest
    {
        public bool GetOnlyNotOpen { get; set; }
        public int PageIndex { get; set;}
        public int PageSize { get; set;}
    }
}
