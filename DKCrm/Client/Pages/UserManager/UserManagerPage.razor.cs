using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;


namespace DKCrm.Client.Pages.UserManager
{
    partial class UserManagerPage
    {
        private List<ApplicationUser> Elements = new List<ApplicationUser>();
        private List<IdentityRole> Roles = new List<IdentityRole>();

        private List<string> _editEvents = new();
        private string _searchString = "";
        private ApplicationUser? _elementBeforeEdit;

        private HashSet<ApplicationUser> _selectedItems = new HashSet<ApplicationUser>();
        private TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.Start;
        private TableEditButtonPosition editButtonPosition = TableEditButtonPosition.Start;
        private TableEditTrigger editTrigger = TableEditTrigger.EditButton;


        private void OpenDialog() => _visibleRegisterDialog = true;
        private DialogOptions _registerDialogOptions = new() { FullWidth = true, CloseButton = true };
        private bool _visibleRegisterDialog;
       
        private ApplicationUser? _currentUser;
        private static List<string>? _currentRoles;
        private DialogOptions _roleDialogOptions = new() { FullWidth = true, CloseButton = true };
        private bool _visibleRoleDialog;
        private string value { get; set; } = "Nothing selected";
        private bool _visibleEditUserProfileDialog;
        private async Task OpenRoleDialog(ApplicationUser user)
        {
            _currentUser = user;
            _currentRoles = await UserManagerCustom.GetRoleFromUser(user.Id);
            value = _currentRoles.FirstOrDefault()!;
            _visibleRoleDialog = true;
        }
        private void OpenEditUserProfileDialog(ApplicationUser user)
        {
            _currentUser = user;
            _visibleEditUserProfileDialog = true;
        }

        protected override async Task OnInitializedAsync()
        {
           
            Elements = await UserManagerCustom.GetUsersAsync();
            Roles = await UserManagerCustom.GetAllRolesAsync();

           
        }

        private void AddEditionEvent(string message)
        {
            _editEvents.Add(message);
            StateHasChanged();
        }

        private void BackupItem(object element)
        {
            _elementBeforeEdit = new()
            {
                Email = ((ApplicationUser)element).Email,
                UserName = ((ApplicationUser)element).UserName,
            };
            AddEditionEvent($"RowEditPreview event: made a backup of Element {((ApplicationUser)element).UserName}");
        }

        private async Task ItemHasBeenCommitted(object element)
        {
            _elementBeforeEdit = new()
            {
                Email = ((ApplicationUser)element).Email,
                UserName = ((ApplicationUser)element).UserName,
            };
            await UserManagerCustom.UpdateUser(_elementBeforeEdit);
           AddEditionEvent($"RowEditCommit event: Changes to Element {((ApplicationUser)element).UserName} committed");
        }

        private void ResetItemToOriginalValues(object element)
        {
            if (_elementBeforeEdit != null)
            {
                ((ApplicationUser)element).Email = _elementBeforeEdit.Email;
                ((ApplicationUser)element).UserName = _elementBeforeEdit.UserName;
            }

            AddEditionEvent($"RowEditCancel event: Editing of Element {((ApplicationUser)element).UserName} canceled");
        }

        private bool FilterFunc(ApplicationUser element)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            if (element.Email.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.UserName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.Id}".Contains(_searchString))
                return true;
            return false;
        }

        [Inject] private IDialogService DialogService { get; set; } = null!;

        private async  Task OnButtonDeleteClicked()
        {
            bool? result = await DialogService.ShowMessageBox(
                "Внимание",
                "Подтвердите удаление!",
                yesText: "Удалить!  ", cancelText: "  Отменить");

            if (result != null && (bool)result)
            {
                RemoveUsers(_selectedItems);
            }
            StateHasChanged();
        }
        private void RemoveUsers(HashSet<ApplicationUser> selectedItems)
        {
            var tempList = new List<ApplicationUser>(selectedItems);
            UserManagerCustom.RemoveUsers(tempList);
            _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }

        private async Task UpdateRoles()
        {
            var request = new RoleRequest() { RoleName = value, UserName = _currentUser?.UserName };
            await UserManagerCustom.UpdateUserRole(request);
            _visibleRoleDialog = false;
        }
    }
}
