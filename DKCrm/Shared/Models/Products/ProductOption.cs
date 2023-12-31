using System.Text.Json.Serialization;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.Products
{
    public class ProductOption:IIdentifiable
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Guid CategoryOptionId { get; set; }
        [JsonIgnore]
        public virtual Product? Product { get; set; }
        public virtual CategoryOption? CategoryOption { get; set; }

    }
}
