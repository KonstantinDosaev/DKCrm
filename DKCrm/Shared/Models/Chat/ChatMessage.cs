
using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.Chat
{
    public class ChatMessage: ISoftDelete
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string FromUserId { get; set; } = null!;
        public Guid ToChatGroupId { get; set; }
        [MaxLength(2400)]
        public string Message { get; set; } = null!;
        public DateTime CreatedDate { get; set; }

        public virtual ApplicationUser? FromUser { get; set; }

        public virtual ChatGroup? ToChatGroup { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        [MaxLength(50)]
        public string? UpdatedUser { get; set; }
    }
}
