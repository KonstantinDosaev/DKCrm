namespace DKCrm.Shared.Models
{
    public class AccessRestriction
    {
        public Guid Id { get; set; }
        public Guid AccessedComponentId { get; set; }
        public  string[] AccessUsersToComponent { get; set; } = null!;
    }
}
