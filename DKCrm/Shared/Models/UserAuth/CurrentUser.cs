namespace DKCrm.Shared.Models.UserAuth
{
    public class CurrentUser
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; } = null!;
        public Dictionary<string, string> Claims { get; set; } = null!;
    }
}
