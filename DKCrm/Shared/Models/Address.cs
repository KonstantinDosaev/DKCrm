using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models
{
    public class Address : IIdentifiable
    {
        public Guid Id { get; set; }
        [Required] public string Country { get; set; } = null!;
        public string? Region { get; set; }
        [Required] public string City { get; set; } = null!;

        [Required] public string Street { get; set; } = null!;

        [Required] public string Home { get; set; } = null!;

        [Required] public string Placement { get; set; } = null!;


        public string? PostalCode { get; set; }

    }
}
