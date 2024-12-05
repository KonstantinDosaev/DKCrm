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
        private List<ChatMessage> _messages = new();
        private List<Guid>? CurrentUserChatGroupsIds { get; set; }
        public List<ApplicationUser> AllChatUsers = new();
        public List<ApplicationUser> NotHavePrivateChatUsers = new();
        [Parameter] public string ContactEmail { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public ApplicationUser? CurrentUser { get; set; }
        [Parameter] public Guid CurrentChatId { get; set; }
        public ChatGroup? CurrentChatGroup { get; set; }
        public List<ChatGroup> ChatGroups = new();
        public List<Guid> SelectedValues = new();
        public List<string> SelectedUser = new();
        private string? CreatedChatName { get; set; }
        private MudForm? _formCreateChatGroup;
        private bool _successFormCreateChatGroup;
        private bool _addUserDialogIsOpen;
        private bool _usersInChatDialogIsOpen;

        private bool _collapseContacts = true;
        private string? ContactsCssClass => _collapseContacts ? "collapse-contacts" : null;
        private bool _collapseMessageBoard = true;
        private string? MessageBoardCssClass => CurrentChatId==Guid.Empty ? "collapse-message" : null;
        protected override async Task OnInitializedAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User.Claims.Where(a => a.Type.Contains("nameidentifier")).Select(a => a.Value).FirstOrDefault()!;
            CurrentUser = await UserManagerCustom.GetUserDetailsAsync(user);

            HubConnection ??= new HubConnectionBuilder().WithUrl(_navigationManager.ToAbsoluteUri("/signalRHub")).Build();

            if (HubConnection.State == HubConnectionState.Disconnected)
                await HubConnection.StartAsync();
            
            HubConnection.On<ChatMessage, string>("ReceiveMessage", async (message, userName) =>
            {
                if (CurrentChatId == message.ToChatGroupId)
                {
                    if ((CurrentChatId == message.ToChatGroupId && CurrentUser.Id != message.FromUserId))
                    {
                        _messages.Add(message);
                        await HubConnection.SendAsync("ChatNotificationAsync", $"Новое сообщение от {userName}", message.ToChatGroupId, message.FromUserId);
                    }
                    else if ((CurrentChatId == message.ToChatGroupId && CurrentUser.Id == message.FromUserId))
                        _messages.Add(message);
                    
                    await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
                    StateHasChanged();
                }
                else if (CurrentUserChatGroupsIds != null && CurrentUserChatGroupsIds.Contains(message.ToChatGroupId))
                {
                    await GetChatGroupsAsync();
                    await HubConnection.SendAsync("ChatNotificationAsync", $"Новое сообщение от {userName}", message.ToChatGroupId, message.FromUserId);
                    StateHasChanged();
                }
            });
            await GetChatGroupsAsync();
            await GetUsersAsync();

            if (CurrentChatId != Guid.Empty) 
                await LoadUserChat(CurrentChatId);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (CurrentChatId!= Guid.Empty && !SelectedValues.Any())
                await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
        }

        private async Task SubmitAsync()
        {
            if (!string.IsNullOrEmpty(CurrentMessage) && !string.IsNullOrEmpty(CurrentChatId.ToString()))
            {
                var chatHistory = new ChatMessage()
                {
                    Message = CurrentMessage,
                    ToChatGroupId = CurrentChatId,
                    FromUserId = CurrentUser!.Id,
                    CreatedDate = DateTime.Now,
                    FromUser = CurrentUser
                };
                await _chatManager.SaveMessageAsync(chatHistory);
                if (HubConnection != null)
                    await HubConnection.SendAsync("SendMessageAsync", chatHistory, $"{CurrentUser.LastName} {CurrentUser.FirstName}");
                CurrentMessage = string.Empty;
            }
        }
        private async Task GetChatGroupsAsync()
        {
            ChatGroups = await _chatManager.GetAllChatsToUser();
            CurrentUserChatGroupsIds = ChatGroups.Select(s => s.Id).ToList();
            ChatGroups = ChatGroups.OrderByDescending(o=>o.DateTimeUpdate).ToList();
            CurrentChatGroup = ChatGroups.FirstOrDefault(f => f.Id == CurrentChatId)!;
        }
        private async Task GetUsersAsync()
        {
            var privateChatsUsersIds = ChatGroups.Where(w => w.IsPrivateGroup).SelectMany(s => s.ApplicationUsers!).Select(s => s.Id).Distinct().ToList();
            AllChatUsers = await _chatManager.GetUsersAsync();
            NotHavePrivateChatUsers = AllChatUsers.Where(user => !privateChatsUsersIds.Contains(user.Id)).ToList();
        }

        private async Task LoadUserChat(Guid chatId)
        {
            CurrentChatId = chatId;
            _navigationManager.NavigateTo($"chat/{CurrentChatId}");
            _messages.Clear();
            _messages = await _chatManager.GetConversationAsync(CurrentChatId);
            await GetChatGroupsAsync();
        }
        private async Task OnClickContact(string userId)
        {
            if (await ConfirmationActionService.ConfirmationActionAsync("Создание беседы"))
                await CreateNewPrivateUserChatAsync(userId);
            
        }
        private async Task CreateNewPrivateUserChatAsync(string userId)
        {
            var createdGroup = await _chatManager.CreateChatGroupAsync(new ChatGroup()
            {
                IsPrivateGroup = true,
            });
            if (createdGroup != Guid.Empty)
                await AddUsersToChatAsync(new List<string>{userId},createdGroup);
           
            CurrentChatId = createdGroup;
            // _navigationManager.NavigateTo($"chat/{CurrentChatId}");
            await GetChatGroupsAsync();
            _messages.Clear();
            _messages = await _chatManager.GetConversationAsync(CurrentChatId);
        }
        private async Task CreateNewGroupUserChatAsync(string chatName)
        {
            if (string.IsNullOrEmpty(chatName))return;
            var createdGroup = await _chatManager.CreateChatGroupAsync(new ChatGroup(){Name = chatName});
            if (createdGroup == Guid.Empty)return;
            CurrentChatId = createdGroup;
            await GetChatGroupsAsync();
                //_navigationManager.NavigateTo($"chat/{CurrentChatId}");
            _messages.Clear();
        }
        private void OpenCloseUsersInChatDialog(bool isOpen)
        {
            SelectedUser.Clear();
            _usersInChatDialogIsOpen = isOpen;
        }
        private void OpenCloseAddUserDialog(bool isOpen)
        { 
            SelectedUser.Clear();
            _usersInChatDialogIsOpen = !_usersInChatDialogIsOpen;
            _addUserDialogIsOpen =isOpen;
        }
        private async Task AddUsersToChatAsync(IEnumerable<string> usersId, Guid chatId)
        {
            usersId = usersId.ToArray();
            if (!usersId.Any()) return;
            var newLogs = usersId.Select(s=> new LogUsersVisitToChat()
            {
                ApplicationUserId = s, ChatGroupId = chatId
            });
            if (await _chatManager.AddUsersToChatAsync(newLogs) != 0)
            {
                
                var userNames = AllChatUsers.Where(w => usersId.Contains(w.Id)).Select(s => s.UserName).ToArray();
                var endChar = userNames.Length > 1 ? "ы" : "(а)";
                CurrentMessage = $"{string.Join(", ", userNames)} добавлен{endChar} в беседу.";
                await SubmitAsync();
                await GetChatGroupsAsync();
            }
            else
            {
                _snackBar.Add("Возникли проблемы при добавлении пользователя в беседу");
            }
            _addUserDialogIsOpen = false;
        }
        private async Task RemoveUsersFromChatAsync(IEnumerable<string> usersId, Guid chatId)
        {
            if (!await ConfirmationActionService.ConfirmationActionAsync("Подтвердите удаление")) return;
            
            var newLogs = usersId.Select(s => new LogUsersVisitToChat()
            {
                ApplicationUserId = s,
                ChatGroupId = chatId
            });
            if (await _chatManager.RemoveUsersFromChatAsync(newLogs) != 0)
            {
              
                var userNames = AllChatUsers.Where(w => usersId.Contains(w.Id)).Select(s => s.UserName).ToArray();
                var endChar = userNames.Length > 1 ? "ы" : "(а)";
                CurrentMessage = $"{string.Join(", ", userNames)} удален{endChar} из беседы.";
                await SubmitAsync();
                await GetChatGroupsAsync();
            }
            _usersInChatDialogIsOpen = false;
        }
        [Inject] private IDialogService DialogService { get; set; } = null!;
        private async void OnButtonDeleteClicked(IEnumerable<Guid> listId)
        {
            var result = await DialogService.ShowMessageBox(
                "Внимание",
                "Подтвердите удаление!",
                yesText: "Удалить!  ", cancelText: "  Отменить");
            
            if (result != null && (bool)result)
                await RemoveMessage(listId);
            
            StateHasChanged();
        }
        private async Task RemoveMessage(IEnumerable<Guid> listId)
        {
            await _chatManager.FoolRemoveMessageAsync(listId);
                _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }
        private async Task RemoveChatGroup(Guid groupId)
        {
            if (await ConfirmationActionService.ConfirmationActionAsync("Подтвердите удаление!!!"))
            {
                await _chatManager.RemoveChatGroupAsync(groupId);
                _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
            }
        }
        private void CheckboxClicked(Guid aSelectedId, object aChecked)
        {
            if ((bool)aChecked && !SelectedValues.Contains(aSelectedId))
                SelectedValues.Add(aSelectedId);

            if (!(bool)aChecked && SelectedValues.Contains(aSelectedId))
                SelectedValues.Remove(aSelectedId);
        }
        private void CheckboxAddUserClicked(string aSelectedId, object aChecked)
        {
            if ((bool)aChecked && !SelectedUser.Contains(aSelectedId))
                SelectedUser.Add(aSelectedId);

            if (!(bool)aChecked && SelectedUser.Contains(aSelectedId))
                SelectedUser.Remove(aSelectedId);
        }
    }
}
