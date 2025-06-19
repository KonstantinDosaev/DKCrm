using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ImportedOrderStatus : IIdentifiable, ISoftDelete
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Value { get; set; } = null!;
        public double Position { get; set; }
        public virtual ICollection<ImportedOrder>? ImportedOrders { get; set; }
        public virtual ICollection<ImportedOrderStatusImportedOrder>? ImportedOrderStatusImportedOrders { get; set; }
        public bool IsValueConstant { get; set; }
        public bool AllowMoveBack { get; set; }
        [MaxLength(400)]
        public string? UsersWitchAccess { get; set; }
        public bool LimitPositionToEditOrder { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        [MaxLength(50)]
        public string? UpdatedUser { get; set; }
        public Guid? ParentId { get; set; }
        public virtual ImportedOrderStatus? Parent { get; set; }
        public virtual ICollection<ImportedOrderStatus>? Children { get; set; }
    }
}
