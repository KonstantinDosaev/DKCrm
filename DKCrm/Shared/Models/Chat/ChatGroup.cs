using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.Chat
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class ChatGroup : ISoftDelete
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string? CreatingUserId { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(200)]
        public string? Image { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        public bool IsPrivateGroup { get; set; }     
        public virtual ICollection<ApplicationUser>? ApplicationUsers { get; set; }
        public virtual ICollection<ChatMessage>? ChatMessages { get; set; }
        public virtual ICollection<LogUsersVisitToChat>? LogUsersVisitToChatList { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        [MaxLength(50)]
        public string? UpdatedUser { get; set; }
    }
}
