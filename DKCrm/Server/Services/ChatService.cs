using DKCrm.Server.Data;
using DKCrm.Shared.Models.Chat;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using DKCrm.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services
{
    public class ChatService : IChatService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserDbContext _context;
        public ChatService(UserManager<ApplicationUser> userManager, UserDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync(ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var allUsers = await _context.Users.Where(user => user.Id != userId).ToListAsync();
            return allUsers;
        }

        public async Task<ApplicationUser> GetUserDetailsAsync(string userId)
        {
            var user = await _context.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
            return user ?? throw new InvalidOperationException();
        }

        public async Task<int> SaveMessageAsync(ChatMessage message, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId != null) message.FromUserId = userId;
            message.CreatedDate = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
            message.ToUser = await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync();
            await _context.ChatMessages.AddAsync(message);
            return  await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetConversationAsync(string contactId, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var messages = await _context.ChatMessages
                .Where(h => (h.FromUserId == contactId && h.ToUserId == userId) || (h.FromUserId == userId && h.ToUserId == contactId))
                .OrderBy(a => a.CreatedDate)
                .Include(a => a.FromUser)
                .Include(a => a.ToUser)
                .Select(x => new ChatMessage
                {
                    FromUserId = x.FromUserId,
                    Message = x.Message,
                    CreatedDate = x.CreatedDate,
                    Id = x.Id,
                    ToUserId = x.ToUserId,
                    ToUser = x.ToUser,
                    FromUser = x.FromUser
                }).ToListAsync();
            return messages;
        }

        public async Task<int> RemoveMessageRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var messages = _context.ChatMessages.Where(w => listId.Contains(w.Id));
            var count = messages.Count();
            _context.ChatMessages.RemoveRange(messages);
            await _context.SaveChangesAsync();
            return count;
        }
    }
}
