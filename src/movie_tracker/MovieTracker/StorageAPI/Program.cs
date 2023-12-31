using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using StorageAPI.AppConfig;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace StorageAPI
{
    public class Program
    {


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);

            builder.Services.Configure<ApiKey>(option => option.Key = builder.Configuration["Api:Key"]);
            
            builder.Services.AddOutputCache(options => options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromMinutes(5))));

            var app = builder.Build();

            app.UseMiddleware<ApiKeyAuthorizationMiddleware>();

            Directory.CreateDirectory("/storage/cover");
            app.MapPut("/put_cover/{id}", Endpoints.CoverHandlers.PutCover).DisableAntiforgery();
            app.MapDelete("/delete_cover/{id}", Endpoints.CoverHandlers.DeleteCover).DisableAntiforgery();

            app.UseMiddleware<HandleMissingFileMiddleware>();
            app.UseOutputCache();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider("/storage/cover"),
                RequestPath = "/cover",
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
                RequestPath = "/static",
            });

            app.Run();
        }



    }


    public class ApiKey
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string? Key { get; set; }
    }
}

