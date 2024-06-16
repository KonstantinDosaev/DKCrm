using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models
{
    public class Address : IIdentifiable
    {
        public Guid Id { get; set; }
        [MaxLength(20)]
        [Required] public string Country { get; set; } = null!;
        [MaxLength(20)]
        public string? Region { get; set; }
        [MaxLength(20)]
        [Required] public string City { get; set; } = null!;
        [MaxLength(20)]
        [Required] public string Street { get; set; } = null!;
        [MaxLength(20)]
        [Required] public string Home { get; set; } = null!;
        [MaxLength(20)]
        [Required] public string Placement { get; set; } = null!;
        [MaxLength(20)]
        public string? PostalCode { get; set; }

    }
}
