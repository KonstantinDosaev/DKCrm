namespace DKCrm.Shared.Iterfaces
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string? UpdatedUser { get; set; }

        public void Undo()
        {
            IsDeleted = false;
        }
    }
}
