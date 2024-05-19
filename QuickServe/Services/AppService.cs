using QuickServe.Entities;
using QuickServe.Services.Interfaces;

namespace QuickServe.Services
{
    public class AppService : BaseService<App>, IAppService
    {
        public AppService(IJsonService jsonService) : base(jsonService, "apps.json")
        {
        }
    }
}
