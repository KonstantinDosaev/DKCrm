using DKCrm.Shared.Models.Chat;
using DKCrm.Shared.Models;
using System.Net.Http.Json;
using System.Security.Claims;

namespace DKCrm.Client.Services.Chat
{
    public class ChatManager : IChatManager
    {
        private readonly HttpClient _httpClient;

        public ChatManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ChatMessage>> GetConversationAsync(Guid chatId)
        {
            return await _httpClient.GetFromJsonAsync<List<ChatMessage>>($"api/chat/{chatId}") ?? throw new InvalidOperationException();
        } 
        public async Task<List<ChatGroup>> GetAllChatsToUser()
        {
            return await _httpClient.GetFromJsonAsync<List<ChatGroup>>($"api/Chat/allChats") ?? throw new InvalidOperationException();
        }
        public async Task<ApplicationUser> GetUserDetailsAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<ApplicationUser>($"api/chat/users/{userId}") ?? throw new InvalidOperationException();
        }

        public async Task RemoveMessageAsync(IEnumerable<Guid> listId)
        {
          await _httpClient.PostAsJsonAsync("api/chat/remove", listId);
        }
        public async Task FoolRemoveMessageAsync(IEnumerable<Guid> listId)
        {
            await _httpClient.PostAsJsonAsync("api/chat/remove-fool", listId);
        }
        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            var data = await _httpClient.GetFromJsonAsync<List<ApplicationUser>>("api/chat/users");
            return data ?? throw new InvalidOperationException();
        }
        public async Task SaveMessageAsync(ChatMessage message)
        {
            await _httpClient.PostAsJsonAsync("api/chat", message);
        } 
        public async Task<Guid> CreateChatGroupAsync(ChatGroup chatGroup)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Chat/add-group", chatGroup);
            return await response.Content.ReadFromJsonAsync<Guid>();
        }
        public async Task<int> RemoveChatGroupAsync(Guid chatGroupId)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Chat/remove-group", chatGroupId);
            return await response.Content.ReadFromJsonAsync<int>();
        }
        public async Task<int> AddUsersToChatAsync(IEnumerable<LogUsersVisitToChat> usersToChat)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Chat/add-users", usersToChat);
            return await response.Content.ReadFromJsonAsync<int>();
        }
        public async Task<int> RemoveUsersFromChatAsync(IEnumerable<LogUsersVisitToChat> usersToChat)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Chat/remove-users", usersToChat);
            return await response.Content.ReadFromJsonAsync<int>();
        }

    }
}
