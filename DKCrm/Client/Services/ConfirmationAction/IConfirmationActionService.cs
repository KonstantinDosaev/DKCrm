namespace DKCrm.Client.Services.ConfirmationAction
{
    public interface IConfirmationActionService
    {
        Task<bool> ConfirmationActionAsync(string message);
    }
}
