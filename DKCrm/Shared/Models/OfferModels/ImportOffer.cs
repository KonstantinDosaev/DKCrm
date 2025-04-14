using DKCrm.Shared.Models.CompanyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.Products;
using DKCrm.Shared.Models.OrderModels;
using System.ComponentModel.DataAnnotations;

namespace DKCrm.Shared.Models.OfferModels
{
    public class ImportOffer: IIdentifiable, ISoftDelete
    {
        public Guid Id { get; set; }
        public double Quantity { get; set; }
        public virtual Company? Company { get; set; }
        public Guid? CompanyId { get; set; }
        public virtual Product? Product { get; set; }
        public Guid? ProductId { get; set; }
        [MaxLength(10)] public string Currency { get; set; } = null!;
        //public virtual ICollection<ImportedProduct>? ImportedProducts { get; set; }
        //public virtual ICollection<ExportedProduct>? ExportedProducts { get; set; }
        public virtual ICollection<PriceForImportOffer>? PricesForImportOffer { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public DateTime? DateTimeCreate { get; set; }
        [MaxLength(50)] public string? UpdatedUser { get; set; }
    }
}
