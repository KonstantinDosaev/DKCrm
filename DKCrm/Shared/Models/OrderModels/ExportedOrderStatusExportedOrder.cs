using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ExportedOrderStatusExportedOrder : ISoftDelete
    {
        public Guid ExportedOrderId { get; set; }
        public virtual ExportedOrder? ExportedOrder { get; set; }

        public Guid ExportedOrderStatusId { get; set; }
        public virtual ExportedOrderStatus? ExportedOrderStatus { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeCreate { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        [MaxLength(50)]
        public string? UpdatedUser { get; set; }
    }
}
