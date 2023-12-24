using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models
{
    public class InternalCompanyData: IIdentifiable
    {
        public Guid Id { get; set; }
        public string? LocalCurrency { get; set; }
        public double CurrencyPercent { get; set; }
        public string? KeyFns { get; set; }
    }
}
