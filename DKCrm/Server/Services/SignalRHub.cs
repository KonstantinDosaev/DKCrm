﻿using DKCrm.Shared.Models.Chat;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.SignalR;

namespace DKCrm.Server.Services
{
    public class SignalRHub : Hub
    {
        public async Task SendMessageAsync(ChatMessage message, string userName)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, userName);
        }
        public async Task ChatNotificationAsync(string message, Guid receiverChatId, string senderUserId)
        {
            await Clients.All.SendAsync("ReceiveChatNotification", message, receiverChatId, senderUserId);
        }
    }
    public class CompanyCommentHub : Hub
    {
        public async Task SendCompanyCommentAsync(CompanyComment comment, string userName)
        {
            await Clients.All.SendAsync("ReceiveCompanyComment", comment, userName);
        }
        public async Task ReloadCompanyCommentList(Guid companyId)
        {
            await Clients.All.SendAsync("ReceiveCommandToReloadCompanyCommentList", companyId);
        }
    }
    public class OrderCommentHub : Hub
    {
       public async Task SendCommentAsync(CommentOrder comment, string userName)
        {
            await Clients.All.SendAsync("ReceiveComment", comment, userName);
        }
        public async Task ReloadCommentList(Guid orderId)
        {
            await Clients.All.SendAsync("ReceiveCommandToReloadCommentList", orderId);
        }
    }
}
