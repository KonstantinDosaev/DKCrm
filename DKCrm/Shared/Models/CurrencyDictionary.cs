using System.ComponentModel.DataAnnotations;

namespace DKCrm.Shared.Models
{
    public class CurrencyDictionary
    {
        public Guid Id { get; set; }
        public bool Male { get; set; }
        public bool MaleLoverNominal { get; set; }
        [MaxLength(7)]
        public string CharCode { get; set; } = null!;
        [MaxLength(15)]
        public string One { get; set; } = null!;
        [MaxLength(15)]
        public string Two { get; set; } = null!;
        [MaxLength(15)]
        public string Five { get; set; } = null!;
        [MaxLength(15)]
        public string OneLoverNominal { get; set; } = null!;
        [MaxLength(15)]
        public string TwoLoverNominal { get; set; } = null!;
        [MaxLength(15)]
        public string FiveLoverNominal { get; set; } = null!;


    }
}
