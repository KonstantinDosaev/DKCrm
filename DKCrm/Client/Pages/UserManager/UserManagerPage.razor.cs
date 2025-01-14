using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;


namespace DKCrm.Client.Pages.UserManager
{
    partial class UserManagerPage
    {
        private List<ApplicationUser> _elements = new List<ApplicationUser>();
        private List<IdentityRole> _roles = new List<IdentityRole>();

        private string _searchString = "";
        private ApplicationUser? _elementBeforeEdit;

        private HashSet<ApplicationUser> _selectedItems = new HashSet<ApplicationUser>();
        private TableApplyButtonPosition _applyButtonPosition = TableApplyButtonPosition.Start;
        private TableEditButtonPosition _editButtonPosition = TableEditButtonPosition.Start;
        private TableEditTrigger _editTrigger = TableEditTrigger.EditButton;
        
        private void OpenDialog() => _visibleRegisterDialog = true;
        private DialogOptions _registerDialogOptions = new() { FullWidth = true, CloseButton = true };
        private bool _visibleRegisterDialog;
       
        private ApplicationUser? _currentUser;
        private static List<string>? _currentRoles;
        private DialogOptions _roleDialogOptions = new() { FullWidth = true, CloseButton = true };
        private bool _visibleRoleDialog;
        private string _value = "Nothing selected";
        private bool _visibleEditUserProfileDialog;
        private async Task OpenRoleDialog(ApplicationUser user)
        {
            _currentUser = user;
            _currentRoles = await UserManagerCustom.GetRoleFromUser(user.Id);
            _value = _currentRoles.FirstOrDefault()!;
            _visibleRoleDialog = true;
        }
        private void OpenEditUserProfileDialog(ApplicationUser user)
        {
            _currentUser = user;
            _visibleEditUserProfileDialog = true;
        }

        protected override async Task OnInitializedAsync()
        {
            _elements = await UserManagerCustom.GetUsersAsync();
            _roles = await UserManagerCustom.GetAllRolesAsync();
        }

        private void BackupItem(object element)
        {
            _elementBeforeEdit = new()
            {
                Email = ((ApplicationUser)element).Email,
                UserName = ((ApplicationUser)element).UserName,
            };
        }

        /*private async Task ItemHasBeenCommitted(object element)
        {
            _elementBeforeEdit = new()
            {
                Email = ((ApplicationUser)element).Email,
                UserName = ((ApplicationUser)element).UserName,
            };
            await UserManagerCustom.UpdateUser(_elementBeforeEdit);
           AddEditionEvent($"RowEditCommit event: Changes to Element {((ApplicationUser)element).UserName} committed");
        }*/

        private void ResetItemToOriginalValues(object element)
        {
            if (_elementBeforeEdit != null)
            {
                ((ApplicationUser)element).Email = _elementBeforeEdit.Email;
                ((ApplicationUser)element).UserName = _elementBeforeEdit.UserName;
            }
        }

        private bool FilterFunc(ApplicationUser element)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            if (element.Email != null && element.Email.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.UserName != null && element.UserName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
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
            var request = new RoleRequest() { RoleName = _value, UserName = _currentUser?.UserName };
            await UserManagerCustom.UpdateUserRole(request);
            _visibleRoleDialog = false;
        }
    }
}
