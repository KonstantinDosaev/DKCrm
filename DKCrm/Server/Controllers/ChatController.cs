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

        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetConversationAsync(string contactId)
        {
            return Ok(await _chatService.GetConversationAsync(contactId, User));
        }
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveMessageRange(IEnumerable<Guid> listId)
        {
            return Ok(await _chatService.RemoveMessageRangeAsync(listId, User));
        }
    }
}
