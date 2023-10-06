using DKCrm.Client.FluentValidation;
using DKCrm.Shared.Models;
using MudBlazor;

namespace DKCrm.Client.Pages.UserManager
{
    partial class UserProfile
    {
       //[Inject] ISnackbar Snackbar { get; set; }

        MudForm UserProfileForm;

        private ApplicationUserModelFluentValidator _applicationUserValidator = new ApplicationUserModelFluentValidator();

        AddressModelFluentValidator _addressValidator = new AddressModelFluentValidator();

        ApplicationUser User = new ApplicationUser();
        protected override async Task OnInitializedAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var userName = state.User.Claims.Where(a => a.Type.Contains("nameidentifier")).Select(a => a.Value).FirstOrDefault()!; ;
            User = await UserManagerCustom.GetUserDetailsAsync(userName);
        }



        private async Task Submit()
        {
            await UserProfileForm.Validate();

            if (UserProfileForm.IsValid)
            {
                await UserManagerCustom.UpdateUser(User);

                _snackBar.Add("Изменения применены!");
            }
        }
    }
}
