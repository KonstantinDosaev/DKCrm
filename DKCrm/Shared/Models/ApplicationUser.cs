using DKCrm.Shared.Models.Chat;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models
{
    public class ApplicationUser : IdentityUser,ISoftDelete
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AdditionalPhoneNumber { get; set; }
        public virtual Address? Address { get; set; }
        public virtual Address? AdditionalAddress { get; set; }
        public Guid? AddressId { get; set; }
        public Guid? AdditionalAddressId { get; set; }
        public virtual ICollection<ChatMessage> ChatMessagesFromUsers { get; set; }
        public virtual ICollection<ChatMessage> ChatMessagesToUsers { get; set; }
        public ApplicationUser()
        {
            ChatMessagesFromUsers = new HashSet<ChatMessage>();
            ChatMessagesToUsers = new HashSet<ChatMessage>();
        }

        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string? UpdatedUser { get; set; }
    }
}
