using QuickServe.Attributes;
using QuickServe.Services.Interfaces;

namespace QuickServe.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppService _appService;
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyMiddleware(RequestDelegate next, IAppService appService, 
            IApiKeyService apiKeyService)
        {
            this._next = next;
            this._appService = appService;
            this._apiKeyService = apiKeyService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // If the endpoint doesn't need to be checked, ignore and continue.
            if (context?.GetEndpoint()?.Metadata.GetMetadata<ApiKeyRequiredAttribute>() == null)
            {
                await _next(context);
                return;
            }

            // If the app does not require authorization, ignore and continue.
            context.Request.RouteValues.TryGetValue("appId", out var appId);
            var app = this._appService.GetById(Guid.Parse(appId?.ToString() ?? ""));
            if ((!app?.RequiresAuthorization) ?? true)
            {
                await _next(context);
                return;
            }

            // If there's no api key supplied, throw a 401.
            if (!context.Request.Headers.TryGetValue("X-API-KEY", out var apiKey)) 
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key is required");
                return;
            }

            // If the api-key is not valid, throw a 401.
            if (!this._apiKeyService.IsKeyValid(apiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key is invalid");
                return;
            }

            await _next(context);
        }
    }
}
