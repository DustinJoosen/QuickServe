
namespace QuickServe.Entities
{
    public class App : IdentifiableEntity
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public string Description { get; set; }
        public bool RequiresAuthorization { get; set; }
    }
}
