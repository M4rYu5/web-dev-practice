using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.Features;
using StorageAPI.AppConfig.Attributes;
using System.Globalization;

namespace StorageAPI.AppConfig
{
    public class ApiKeyAuthorizationMiddleware(RequestDelegate next, ApiKey apiKey)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var customAuth = context.GetEndpoint()?.Metadata.GetMetadata<ApiKeyAuthorizationAttribute>();
            if (customAuth != null)
            {
                if (string.IsNullOrEmpty(apiKey.Key))
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    return;
                }
                
                var providedKey = context.Request.Headers["api-key"];
                if (string.IsNullOrEmpty(providedKey) || providedKey != apiKey.Key)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
            await next(context);
        }
    }
}
