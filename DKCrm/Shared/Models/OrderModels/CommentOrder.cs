using DKCrm.Shared.Models.Chat;
using System.ComponentModel.DataAnnotations;

namespace DKCrm.Shared.Models.OrderModels
{
    public class CommentOrder
    {
        public Guid Id { get; set; }
        public bool IsWarningComment { get; set; }
        [MaxLength(2400)]
        public string Value { get; set; } = null!;
        [MaxLength(50)]
        public string FromUserId { get; set; } = null!;
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeUpdate { get; set; }
        public Guid OrderId { get; set; }
        [MaxLength(50)]
        public string? OrderType { get; set; }
    }
}
