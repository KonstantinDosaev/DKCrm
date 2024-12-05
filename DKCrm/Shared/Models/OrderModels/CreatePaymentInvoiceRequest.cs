using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Shared.Models.OrderModels
{
    public class CreatePaymentInvoiceRequest
    {
        public  Guid OrderId { get; set; }
        public string ContractNumber { get; set; } = null!;
        public string IGK { get; set; } = null!;
        public DateTime? ContractDate { get; set; } = DateTime.Now;
        public BankDetails OurSelectedBank{ get; set; } = null!;
        public BankDetails BuyerSelectedBank { get; set; } = null!;
        //public ICollection<StringToFonts> NamingCondition { get; set; } = null!;
        //public ICollection<StringToFonts> Conditions { get; set; } = null!;
    }
}
