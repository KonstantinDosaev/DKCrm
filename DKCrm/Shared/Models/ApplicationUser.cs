using System.ComponentModel.DataAnnotations;
using DKCrm.Shared.Models.Chat;
using Microsoft.AspNetCore.Identity;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models
{
    public class ApplicationUser : IdentityUser,ISoftDelete
    {
        [MaxLength(20)]
        public string? FirstName { get; set; }
        [MaxLength(20)]
        public string? LastName { get; set; }
        [MaxLength(20)]
        public string? AdditionalPhoneNumber { get; set; }
        public virtual Address? Address { get; set; }
        public virtual Address? AdditionalAddress { get; set; }
        public Guid? AddressId { get; set; }
        public Guid? AdditionalAddressId { get; set; }
        public bool AreThereNewMessages { get; set; }
        public bool AreThereNewOrderComments { get; set; }
        public virtual ICollection<LogUsersVisitToChat>? LogUsersVisitToChatList { get; set; }
        public virtual ICollection<ChatGroup>? ChatGroups { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        [MaxLength(50)]
        public string? UpdatedUser { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
