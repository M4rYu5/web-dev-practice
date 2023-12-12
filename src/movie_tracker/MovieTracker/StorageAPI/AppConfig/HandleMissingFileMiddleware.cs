using Microsoft.Extensions.Options;
using StorageAPI.AppConfig.Attributes;
using System.Text;

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

            // remove last /, if existent
            var filteredPath = context.Request.Path.Value.EndsWith('/') ? context.Request.Path.Value[..^1] : context.Request.Path.Value;

            // add .png if needed
            if (!filteredPath.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
            {
                filteredPath += ".png";
            }
            context.Request.Path = filteredPath;

            await _next(context);

            // return default cover if nothing was found
            if (context.Response.StatusCode == 404 && is_get_cover)
            {
                context.Response.Clear();
                context.Request.Path = filteredPath.EndsWith("-full.png") ? "/static/cover-default-1000.png" : "/static/cover-default-150.png";
                await _next(context);
            }
        }
    }
}
