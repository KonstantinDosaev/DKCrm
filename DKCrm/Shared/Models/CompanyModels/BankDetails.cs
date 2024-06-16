using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class BankDetails : IIdentifiable
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [MaxLength(30)]
        public string? Inn { get; set; }
        [MaxLength(50)]
        public string BankAccount { get; set; } = null!;
        [MaxLength(50)]
        public string BeneficiaryAccount { get; set; } = null!;
        [MaxLength(200)]
        public string? Description { get; set; }

        public virtual Company? Company { get; set; }
        public  Guid CompanyId { get; set; }

    }
}
