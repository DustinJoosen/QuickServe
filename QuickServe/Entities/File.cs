namespace QuickServe.Entities
{
    public class File : IdentifiableEntity
    {
        public Guid AppId { get; set; }
        public string FileName { get; set; }
    }
}
