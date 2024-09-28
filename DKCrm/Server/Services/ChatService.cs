using DKCrm.Server.Data;
using DKCrm.Shared.Models.Chat;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using DKCrm.Server.Interfaces;
using DKCrm.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using DateTime = System.DateTime;

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
            var allUsers = await _context.Users.Where(u => u.Id != userId).ToListAsync();
            return allUsers;
        }

        public async Task<ApplicationUser> GetUserDetailsAsync(string userId)
        {
            var user = await _context.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
            return user ?? throw new InvalidOperationException();
        }
        public async Task<IEnumerable<ChatGroup>> GetAllChatGroupsToUser(ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var chatGroups = await _context.ChatGroups.Where(w => w.ApplicationUsers!.Select(s => s.Id).Contains(userId))
                .Include(i => i.ApplicationUsers)
                .Include(i=>i.LogUsersVisitToChatList).ToListAsync();
            return chatGroups;
        }
        public async Task<IEnumerable<ChatMessage>> GetConversationAsync(Guid chatId, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var logVisits = _context.LogUsersVisitToChats.FirstOrDefault(w =>
                    w.ApplicationUserId == userId & w.ChatGroupId == chatId);
            if (logVisits != null)
            {
                logVisits.DateTimeVisit = DateTime.UtcNow.AddHours(3);
                _context.Entry(logVisits).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                return new List<ChatMessage>();
            }
            var messages = await _context.ChatMessages
                .Where(h => h.ToChatGroupId == chatId)
                .OrderBy(a => a.CreatedDate)
                .Include(a => a.FromUser)
                .Include(a => a.ToChatGroup)
                .Select(x => new ChatMessage
                {
                    FromUserId = x.FromUserId,
                    Message = x.Message,
                    CreatedDate = x.CreatedDate,
                    Id = x.Id,
                    ToChatGroupId = x.ToChatGroupId,
                    ToChatGroup = x.ToChatGroup,
                    FromUser = x.FromUser
                }).ToListAsync();
            return messages;
        }

        public async Task<int> SaveMessageAsync(ChatMessage message, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId != null) message.FromUserId = userId;
            message.CreatedDate = DateTime.UtcNow.AddHours(3);
            var currentChatGroup = await _context.ChatGroups.Where(w => w.Id == message.ToChatGroupId)
                .Include(i=>i.LogUsersVisitToChatList).FirstOrDefaultAsync();
            if (currentChatGroup != null)
            {
                var currentDateTime = DateTime.UtcNow.AddHours(3);
                currentChatGroup.DateTimeUpdate =currentDateTime;
                _context.Entry(currentChatGroup).State = EntityState.Modified;
                message.ToChatGroup = currentChatGroup;
                var logVisit = currentChatGroup.LogUsersVisitToChatList?.FirstOrDefault(f=>f.ApplicationUserId == userId);
                if (logVisit != null)
                {
                    logVisit.DateTimeVisit = currentDateTime;
                    _context.Entry(logVisit).State = EntityState.Modified;
                }
            }
            _context.Entry(message).State = EntityState.Added;
            return  await _context.SaveChangesAsync();
        }
        public async Task<Guid> CreateChatGroupAsync(ChatGroup chatGroup, ClaimsPrincipal user)
        {
            var currentUserId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (currentUserId != null)
            {
                var userInDb = await _userManager.FindByIdAsync(currentUserId);
                if (userInDb != null) chatGroup.ApplicationUsers = new List<ApplicationUser>() { userInDb };
            }

            chatGroup.CreatingUserId = currentUserId;
            chatGroup.DateTimeUpdate = DateTime.Now;
            await _context.ChatGroups.AddAsync(chatGroup);
            await _context.SaveChangesAsync();
            return chatGroup.Id;
        }
        public async Task<int> RemoveChatGroupAsync(Guid chatGroupId, ClaimsPrincipal currentUser)
        {
            var chatGroup = await _context.ChatGroups.FirstOrDefaultAsync(w => w.Id == chatGroupId);
            var currentUserId = currentUser.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier)
                .Select(a => a.Value).FirstOrDefault();
            if (currentUserId != chatGroup?.CreatingUserId || chatGroup == null) 
                return 0;
            _context.ChatGroups.Remove(chatGroup);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> AddUsersToChatAsync(List<LogUsersVisitToChat> usersToChat, ClaimsPrincipal currentUser)
        {
            var chatGroup = await _context.ChatGroups.Where(w=> w.Id == usersToChat.Select(s=>s.ChatGroupId).FirstOrDefault())
                .Include(i=>i.ApplicationUsers).FirstOrDefaultAsync();
            var currentUserId = currentUser.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (currentUserId != chatGroup?.CreatingUserId) return 0;

            var addUsersIds = usersToChat.Select(s => s.ApplicationUserId).ToArray();
            var addUsers = _userManager.Users.Where(w => addUsersIds.Contains(w.Id)).ToArray();
            if (chatGroup == null || !addUsers.Any()) return 0;
            chatGroup.ApplicationUsers ??= new List<ApplicationUser>();
            var presentInChatGroupUsersIds = chatGroup.ApplicationUsers.Select(s => s.Id).ToArray();
            foreach (var user in addUsers)
            {
                if(!presentInChatGroupUsersIds.Contains(user.Id))
                    chatGroup.ApplicationUsers.Add(user);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveUsersFromChatAsync(List<LogUsersVisitToChat> usersToChat, ClaimsPrincipal currentUser)
        {
            var chatGroup = await _context.ChatGroups.Where(w => w.Id == usersToChat.Select(s => s.ChatGroupId).FirstOrDefault())
                .Include(i => i.ApplicationUsers).FirstOrDefaultAsync();
            var currentUserId = currentUser.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (currentUserId != chatGroup?.CreatingUserId) return 0;

            var removeUsersIds = usersToChat.Select(s => s.ApplicationUserId).ToList();
            if (removeUsersIds.Contains(currentUserId!))
                removeUsersIds.Remove(currentUserId!);
            
            var users = _userManager.Users.Where(w => removeUsersIds.Contains(w.Id)).ToArray();
            if (chatGroup?.ApplicationUsers == null || !users.Any()) return 0;
            foreach (var user in users)
            {
                chatGroup.ApplicationUsers.Remove(user);
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<int> RemoveMessageRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user)
        {
            //var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var messages = _context.ChatMessages.Where(w => listId.Contains(w.Id));
            var count = messages.Count();
            _context.ChatMessages.RemoveRange(messages);
            await _context.SaveChangesAsync();
            return count;
        }
        public async Task<int> FoolRemoveMessageRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user)
        {
            var userRole = user.Claims.Where(a => a.Type == ClaimTypes.Role).Select(a => a.Value).ToArray();
            if (userRole.Contains(RoleNames.Admin) || userRole.Contains(RoleNames.SuAdmin))
            {
                var messages = _context.ChatMessages.Where(w => listId.Contains(w.Id));
                foreach (var message in messages)
                {
                    message.IsFullDeleted = true;
                }
                var count = messages.Count();
                _context.ChatMessages.RemoveRange(messages);
                await _context.SaveChangesAsync();
                return count;
            }
            return 0;
        }
    }
}
