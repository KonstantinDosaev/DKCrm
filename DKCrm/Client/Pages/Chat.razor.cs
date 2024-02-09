using DKCrm.Shared.Models.Chat;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;

namespace DKCrm.Client.Pages
{
    partial class Chat
    {

        [CascadingParameter] public HubConnection? HubConnection { get; set; }
        [Parameter] public string CurrentMessage { get; set; } = null!;
        [Parameter] public string CurrentUserId { get; set; } = null!;
        [Parameter] public string CurrentUserEmail { get; set; } = null!;
        private List<ChatMessage> messages = new List<ChatMessage>();

        public List<ApplicationUser> ChatUsers = new List<ApplicationUser>();
        [Parameter] public string ContactEmail { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string CurrentUserName { get; set; } = null!;
        [Parameter] public string ContactId { get; set; } = null!;

        public List<Guid> SelectedValues = new();
        //private bool _checked;
        //private Guid _value;
        protected override async Task OnInitializedAsync()
        {
            if (HubConnection == null)
            {
                HubConnection = new HubConnectionBuilder().WithUrl(_navigationManager.ToAbsoluteUri("/signalRHub")).Build();
            }
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
            HubConnection.On<ChatMessage, string>("ReceiveMessage", async (message, userName) =>
            {
                if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId) || (ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
                {

                    if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId))
                    {
                        messages.Add(new ChatMessage { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser() { Email = CurrentUserEmail } });
                        await HubConnection.SendAsync("ChatNotificationAsync", $"Новое сообщение от {userName}", ContactId, CurrentUserId);
                    }
                    else if ((ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
                    {
                        messages.Add(new ChatMessage { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new ApplicationUser() { Email = ContactEmail } });
                    }
                    await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
                    StateHasChanged();
                }
            });
            await GetUsersAsync();
            var state = await _stateProvider.GetAuthenticationStateAsync();

            var user = state.User;
            CurrentUserName = user.Claims.Where(a => a.Type.Contains("name")).Select(a => a.Value).FirstOrDefault()!;
            CurrentUserId = user.Claims.Where(a => a.Type.Contains("nameidentifier")).Select(a => a.Value).FirstOrDefault()!;
            CurrentUserEmail = user.Claims.Where(a => a.Type.Contains("emailaddress")).Select(a => a.Value).FirstOrDefault()!;

            if (!string.IsNullOrEmpty(ContactId))
            {
                await LoadUserChat(ContactId);
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
        }

        private async Task SubmitAsync()
        {
            if (!string.IsNullOrEmpty(CurrentMessage) && !string.IsNullOrEmpty(ContactId))
            {
                var chatHistory = new ChatMessage()
                {
                    Message = CurrentMessage,
                    ToUserId = ContactId,
                    FromUserId = CurrentUserId,
                    CreatedDate = DateTime.Now
                };
                await _chatManager.SaveMessageAsync(chatHistory);
                chatHistory.FromUserId = CurrentUserId;
                if (HubConnection != null)
                    await HubConnection.SendAsync("SendMessageAsync", chatHistory, CurrentUserEmail);
                CurrentMessage = string.Empty;
            }
        }

        private async Task LoadUserChat(string userId)
        {
            var contact = await _chatManager.GetUserDetailsAsync(userId);
            ContactId = contact.Id;
            ContactEmail = contact.Email;
            ContactName = contact.UserName;
            _navigationManager.NavigateTo($"chat/{ContactId}");
            messages = new List<ChatMessage>();
            messages = await _chatManager.GetConversationAsync(ContactId);
        }
        private async Task GetUsersAsync()
        {
            ChatUsers = await _chatManager.GetUsersAsync();
        }

        [Inject] private IDialogService DialogService { get; set; } = null!;
        private async void OnButtonDeleteClicked(IEnumerable<Guid> listId)
        {
            var result = await DialogService.ShowMessageBox(
                "Внимание",
                "Подтвердите удаление!",
                yesText: "Удалить!  ", cancelText: "  Отменить");
            
            if (result != null && (bool)result)
            {
                await RemoveMessage(listId);
            }
            StateHasChanged();
        }
        private async Task RemoveMessage(IEnumerable<Guid> listId)
        {
                await _chatManager.RemoveMessageAsync(listId);
                _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }
        private void CheckboxClicked(Guid aSelectedId, object aChecked)
        {
            if ((bool)aChecked)
            {
                if (!SelectedValues.Contains(aSelectedId))
                {
                    SelectedValues.Add(aSelectedId);
                }
            }
            else
            {
                if (SelectedValues.Contains(aSelectedId))
                {
                    SelectedValues.Remove(aSelectedId);
                }
            }
        }
    }
}
