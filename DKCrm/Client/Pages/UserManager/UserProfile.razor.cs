using DKCrm.Client.FluentValidation;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DKCrm.Client.Pages.UserManager
{
    partial class UserProfile
    {
        MudForm? UserProfileForm;

        private readonly ApplicationUserModelFluentValidator _applicationUserValidator = new ApplicationUserModelFluentValidator();

        AddressModelFluentValidator _addressValidator = new AddressModelFluentValidator();

        [Parameter]public ApplicationUser? User { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (User == null)
            {
                var state = await _stateProvider.GetAuthenticationStateAsync();
                var userId = state.User.Claims.Where(a => a.Type.Contains("nameidentifier")).Select(a => a.Value)
                    .FirstOrDefault()!;
                User = await UserManagerCustom.GetUserDetailsAsync(userId);
            }
        }



        private async Task Submit()
        {
            if (UserProfileForm != null)
            {
                await UserProfileForm.Validate();

                if (UserProfileForm.IsValid)
                {
                    if (!await ConfirmationActionService.ConfirmationActionAsync("Подтвердите сохранение"))
                        return;

                    await UserManagerCustom.UpdateUser(User!);

                    _snackBar.Add("Изменения применены!");
                }
            }
        }
    }
}
