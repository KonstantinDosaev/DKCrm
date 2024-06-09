
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.Chat
{
    public class ChatGroup : ISoftDelete
    {
        public Guid Id { get; set; }
        public string? CreatingUserId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public bool IsPrivateGroup { get; set; }     
        public virtual ICollection<ApplicationUser>? ApplicationUsers { get; set; }
        public virtual ICollection<ChatMessage>? ChatMessages { get; set; }
        public virtual ICollection<LogUsersVisitToChat>? LogUsersVisitToChatList { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string? UpdatedUser { get; set; }
    }
}
