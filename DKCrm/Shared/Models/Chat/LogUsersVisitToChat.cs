using System.ComponentModel.DataAnnotations;

namespace DKCrm.Shared.Models.Chat
{
    public class LogUsersVisitToChat
    {
        public DateTime DateTimeVisit { get; set; }
        [MaxLength(50)]
        public string ApplicationUserId { get; set; } = null!;
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public Guid ChatGroupId { get; set; }
        public virtual ChatGroup? ChatGroup { get; set; }
    }
}
