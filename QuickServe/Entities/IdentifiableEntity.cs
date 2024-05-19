namespace QuickServe.Entities
{
    public abstract class IdentifiableEntity
    {
        public Guid Uuid { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}


