using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.OrderModels
{
    public class FilterOrderTuple
    {
        public List<Guid>? OurCompanies { get; set; }
        public List<Guid>? ContragentsCompanies { get; set; }
    }
}
