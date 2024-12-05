using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class FnsRequest: IIdentifiable
    {
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [MaxLength(30)]
        public string INN { get; set; } = null!;
        [MaxLength(30)]
        public string KPP { get; set; } = null!;
        [MaxLength(30)]
        public string PostalCode { get; set; } = null!;
        [MaxLength(30)]
        public string Phone { get; set; } = null!;
        [MaxLength(30)]
        public string ORGN { get; set; } = null!;
        [MaxLength(50)]
        public string Director { get; set; } = null!;
        [MaxLength(200)]
        public string OKVED { get; set; } = null!;
        [MaxLength(50)]
        public string? Revenue { get; set; }
        [MaxLength(200)]
        public string? LegalAddress { get; set; }

        public virtual Company? Company { get; set; }
        public Guid? CompanyId { get; set; }

    }
}
