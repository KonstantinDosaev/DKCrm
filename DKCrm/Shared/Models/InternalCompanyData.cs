using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models
{
    public class InternalCompanyData: IIdentifiable
    {
        public Guid Id { get; set; }
        [MaxLength(7)]
        public string? LocalCurrency { get; set; }
        public double Nds { get; set; }
        public double CurrencyPercent { get; set; }
        [MaxLength(50)]
        public string? KeyFns { get; set; }
    }
}
