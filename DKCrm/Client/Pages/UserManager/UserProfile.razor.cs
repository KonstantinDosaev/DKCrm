using DKCrm.Client.FluentValidation;
using DKCrm.Shared.Models;
using MudBlazor;

namespace DKCrm.Client.Pages.UserManager
{
    partial class UserProfile
    {
        MudForm? UserProfileForm;

        private readonly ApplicationUserModelFluentValidator _applicationUserValidator = new ApplicationUserModelFluentValidator();

        AddressModelFluentValidator _addressValidator = new AddressModelFluentValidator();

        private ApplicationUser _user = new ();
        protected override async Task OnInitializedAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var userName = state.User.Claims.Where(a => a.Type.Contains("nameidentifier")).Select(a => a.Value).FirstOrDefault()!; ;
            _user = await UserManagerCustom.GetUserDetailsAsync(userName);
        }



        private async Task Submit()
        {
            if (!await ConfirmationActionService.ConfirmationActionAsync("Подтвердите сохранение"))
                return;
            if (UserProfileForm != null)
            {
                await UserProfileForm.Validate();

                if (UserProfileForm.IsValid)
                {
                    await UserManagerCustom.UpdateUser(_user);

                    _snackBar.Add("Изменения применены!");
                }
            }
        }
    }
}
