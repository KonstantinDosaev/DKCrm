using DKCrm.Server.Data;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DKCrm.Server.Interfaces;
using DKCrm.Shared.Models.Chat;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            return Ok(await _chatService.GetUsersAsync(User));
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserDetailsAsync(string userId)
        {
            return Ok(await _chatService.GetUserDetailsAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveMessageAsync(ChatMessage message)
        {
            return Ok(await _chatService.SaveMessageAsync(message, User));
        }
        [HttpPost("add-group")]
        public async Task<IActionResult> CreateChatGroup(ChatGroup chatGroup)
        {
            return Ok(await _chatService.CreateChatGroupAsync(chatGroup, User));
        }
        [HttpPost("remove-group")]
        public async Task<IActionResult> RemoveChatGroup(Guid chatGroupId)
        {
            return Ok(await _chatService.RemoveChatGroupAsync(chatGroupId, User));
        }
        [HttpPost("add-users")]
        public async Task<IActionResult> AddUsersToChat(List<LogUsersVisitToChat> usersToChat)
        {
            return Ok(await _chatService.AddUsersToChatAsync(usersToChat, User));
        }
        [HttpPost("remove-users")]
        public async Task<IActionResult> RemoveUsersFromChat(List<LogUsersVisitToChat> usersToChat)
        {
            return Ok(await _chatService.RemoveUsersFromChatAsync(usersToChat, User));
        }
        [HttpGet("{chatId:guid}")]
        public async Task<IActionResult> GetConversationAsync(Guid chatId)
        {
            return Ok(await _chatService.GetConversationAsync(chatId, User));
        }
        [HttpGet("allChats")]
        public async Task<IActionResult> GetAllChatGroupsToUser()
        {
            return Ok(await _chatService.GetAllChatGroupsToUser(User));
        }
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveMessageRange(IEnumerable<Guid> listId)
        {
            return Ok(await _chatService.RemoveMessageRangeAsync(listId, User));
        }
        [HttpPost("remove-fool")]
        public async Task<IActionResult> FoolRemoveMessageRange(IEnumerable<Guid> listId)
        {
            return Ok(await _chatService.FoolRemoveMessageRangeAsync(listId, User));
        }
    }
}
