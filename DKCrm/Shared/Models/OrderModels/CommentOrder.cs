using System.ComponentModel.DataAnnotations;

namespace DKCrm.Shared.Models.OrderModels
{
    public class CommentOrder
    {
        public Guid Id { get; set; }
        [MaxLength(2400)]
        public string Value { get; set; } = null!;
        [MaxLength(50)]
        public string FromUserId { get; set; } = null!;
        public DateTime DateTimeCreated { get; set; }
        public Guid OrderId { get; set; }
    }
}
