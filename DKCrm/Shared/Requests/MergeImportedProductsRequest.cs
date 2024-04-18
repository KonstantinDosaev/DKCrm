using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Shared.Requests
{
    public class MergeImportedProductsRequest
    {
        public Guid PrimaryImportedProductId { get; set; } 

        public Guid SecondaryImportedProductId { get; set; }
    }
}
