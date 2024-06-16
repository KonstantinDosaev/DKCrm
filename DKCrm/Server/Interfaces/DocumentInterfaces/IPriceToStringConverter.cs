namespace DKCrm.Server.Interfaces.DocumentInterfaces
{
    public interface IPriceToStringConverter
    {
        Task<string> ConvertNumber(decimal value, string charCode);
    }
}
