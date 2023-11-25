using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
            app.MapPut("/cover/{id}", Endpoints.CoverHandlers.PutCover).DisableAntiforgery();
            app.MapDelete("/cover/{id}", Endpoints.CoverHandlers.DeleteCover);

            app.UseOutputCache();
            app.UseStaticFiles("/cover");

            app.Run();
        }



    }


    public class ApiKey
    {
        public string? Key { get; set; }
    }
}

