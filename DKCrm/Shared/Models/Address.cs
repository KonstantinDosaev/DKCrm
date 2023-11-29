using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models
{
    public class Address : IIdentifiable
    {
        public Guid Id { get; set; }
        [Required]
        public string Country { get; set; }
        public string? Region { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Home { get; set; }

        [Required]
        public string Placement { get; set; }


        public string? PostalCode { get; set; }

    }
}
