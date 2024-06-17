using QuickServe.Entities;
using QuickServe.Services.Interfaces;

namespace QuickServe.Services
{
    public class ApiKeyService : BaseService<ApiKey>, IApiKeyService
    {
        public ApiKeyService(IJsonService jsonService) : base(jsonService, "apikeys.json")
        {
        }

        public string GenerateApiKey()
        {
            // Generate a new guid by mashing together two guids and removing the hyphens.
            var newGuid = $"{Guid.NewGuid()}{Guid.NewGuid()}".Replace("-", string.Empty);

            // Store the new api keys
            this.Create(new ApiKey
            {
                Key = newGuid
            });

            return newGuid;
        }

        public bool IsKeyValid(string key) =>
            this.GetAll().Any(apiKey => apiKey.Key == key);

    }
}
