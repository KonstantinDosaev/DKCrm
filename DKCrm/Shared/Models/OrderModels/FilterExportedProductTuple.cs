using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.OrderModels
{
    public class FilterExportedProductTuple
    {
        public bool GroupByOrder { get; set; } 
        public bool IsNotComplete { get; set; }
        public FilterOrderTuple? FilterOrderTuple { get; set; }
    }
}
