using MudBlazor;

namespace DKCrm.Client.Services.ConfirmationAction
{
    public class ConfirmationActionService : IConfirmationActionService
    {
        public ConfirmationActionService(IDialogService dialogService)
        {
            DialogService = dialogService;
        }

        public IDialogService DialogService { get; set; } 

        public async Task<bool> ConfirmationActionAsync(string message)
        {
            var result = await DialogService.ShowMessageBox(
                "Внимание",
                $"{message}",
                yesText: "Подтвердить  ", cancelText: "  Отменить");

            if (result != null && (bool)result)
            {
               return (bool)result;
            }
            return false;
        }
    }
}
