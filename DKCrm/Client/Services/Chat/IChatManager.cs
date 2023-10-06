using DKCrm.Shared.Models.Chat;
using DKCrm.Shared.Models;

namespace DKCrm.Client.Services.Chat
{
    public interface IChatManager
    {
        Task<List<ApplicationUser>> GetUsersAsync();
        Task SaveMessageAsync(ChatMessage message);
        Task<List<ChatMessage>> GetConversationAsync(string contactId);
        Task<ApplicationUser> GetUserDetailsAsync(string userId);
        Task RemoveMessageAsync(IEnumerable<Guid> listId);
    }
}
