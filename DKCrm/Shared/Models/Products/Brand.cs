using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.Products
{
    public class Brand:IIdentifiable
    {
        public Guid Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; } = null!;
        [MaxLength(200)]
        public string? Image { get; set; } = null!;
        [MaxLength(2000)]
        public string? Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }
    }
}
