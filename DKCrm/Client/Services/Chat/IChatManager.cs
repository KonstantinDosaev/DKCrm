using DKCrm.Shared.Models.Chat;
using DKCrm.Shared.Models;

namespace DKCrm.Client.Services.Chat
{
    public interface IChatManager
    {
        Task<List<ApplicationUser>> GetUsersAsync();
        Task SaveMessageAsync(ChatMessage message);
        Task<List<ChatMessage>> GetConversationAsync(Guid chatId);
        Task<List<ChatGroup>> GetAllChatsToUser();
        Task<Guid> CreateChatGroupAsync(ChatGroup chatGroup);
        Task<ApplicationUser> GetUserDetailsAsync(string userId);
        Task RemoveMessageAsync(IEnumerable<Guid> listId);
        Task FoolRemoveMessageAsync(IEnumerable<Guid> listId);
        Task<int> RemoveChatGroupAsync(Guid chatGroupId);
        Task<int> AddUsersToChatAsync(IEnumerable<LogUsersVisitToChat> usersToChat);
        Task<int> RemoveUsersFromChatAsync(IEnumerable<LogUsersVisitToChat> usersToChat);
    }
}
