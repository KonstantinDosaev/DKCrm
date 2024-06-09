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
       Task<IEnumerable<ChatMessage>> GetConversationAsync(Guid chatId, ClaimsPrincipal user);
       Task<IEnumerable<ChatGroup>> GetAllChatGroupsToUser(ClaimsPrincipal user);
       Task<Guid> CreateChatGroupAsync(ChatGroup chatGroup, ClaimsPrincipal user);
       Task<int> RemoveChatGroupAsync(Guid chatGroupId, ClaimsPrincipal currentUser);
       Task<int> AddUsersToChatAsync(List<LogUsersVisitToChat> usersToChat, ClaimsPrincipal currentUser);
       Task<int> RemoveUsersFromChatAsync(List<LogUsersVisitToChat> usersToChat, ClaimsPrincipal currentUser);
        Task<int> RemoveMessageRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user);
        Task<int> FoolRemoveMessageRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user);
    }
}
