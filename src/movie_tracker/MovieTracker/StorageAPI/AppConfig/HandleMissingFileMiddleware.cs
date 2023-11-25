using Microsoft.Extensions.Options;
using StorageAPI.AppConfig.Attributes;

namespace StorageAPI.AppConfig
{
    public class HandleMissingFileMiddleware
    {
        private readonly RequestDelegate _next;

        public HandleMissingFileMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // filter for /cover paths
            var is_get_cover = context.Request.Path.StartsWithSegments("/cover") && context.Request.Method.Equals("get", StringComparison.InvariantCultureIgnoreCase);
            if (context.Request.Path.Value == null || !is_get_cover)
            {
                await _next(context);
                return;
            }

            // add .png if needed
            if (!(context.Request.Path.Value.EndsWith(".png") || context.Request.Path.Value.EndsWith(".png/")))
            {
                context.Request.Path += ".png";
            }

            await _next(context);

            // return default cover if nothing was found
            if (context.Response.StatusCode == 404 && is_get_cover)
            {
                context.Request.Path = "/static/cover-default-150.png";
                await _next(context);
            }
        }
    }
}
