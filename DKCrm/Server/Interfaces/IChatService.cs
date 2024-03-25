using DKCrm.Shared.Models.Chat;
using DKCrm.Shared.Models;
using System.Security.Claims;

namespace DKCrm.Server.Interfaces
{
    public interface IChatService
    {
       Task<IEnumerable<ApplicationUser>> GetUsersAsync(ClaimsPrincipal user);
       Task<ApplicationUser> GetUserDetailsAsync(string userId);
       Task<int> SaveMessageAsync(ChatMessage message, ClaimsPrincipal user);
       Task<IEnumerable<ChatMessage>> GetConversationAsync(string contactId, ClaimsPrincipal user);
       Task<int> RemoveMessageRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user);
    }
}
