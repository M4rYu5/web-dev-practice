using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using StorageAPI.AppConfig.Attributes;
using System.Globalization;

namespace StorageAPI.AppConfig
{
    public class ApiKeyAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiKey _apiKey;

        public ApiKeyAuthorizationMiddleware(RequestDelegate next, IOptions<ApiKey> apiKey)
        {
            _next = next;
            _apiKey = apiKey.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var customAuth = context.GetEndpoint()?.Metadata.GetMetadata<ApiKeyAuthorizationAttribute>();
            if (customAuth != null)
            {
                if (string.IsNullOrEmpty(_apiKey.Key))
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    return;
                }

                var providedKey = context.Request.Headers["api-key"];
                if (string.IsNullOrEmpty(providedKey) || providedKey != _apiKey.Key)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
            await _next(context);
        }
    }
}
