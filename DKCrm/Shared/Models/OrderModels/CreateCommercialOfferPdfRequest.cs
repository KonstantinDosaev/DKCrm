using DKCrm.Shared.Models.CompanyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.OrderModels
{
    public class CreateCommercialOfferPdfRequest
    {
        public Guid OrderId { get; set; }
        public BankDetails OurSelectedBank { get; set; } = null!;
        public byte[]? Avatar { get; set; }
    }
}
