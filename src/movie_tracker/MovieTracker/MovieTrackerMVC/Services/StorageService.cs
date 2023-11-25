using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Net.Http;

namespace MovieTrackerMVC.Services
{
    public class StorageService
    {
        private readonly HttpClient httpClient;

        public StorageService(HttpClient httpClient, IOptions<StorageOptions> appConfig)
        {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri(appConfig.Value.StorageAddress);
            this.httpClient.DefaultRequestHeaders.Add("API-KEY", appConfig.Value.StorageKey);
        }

        public async Task UploadCover()
        {
            using var fileStream = File.OpenRead("test.png");
            using var fileContent = new StreamContent(fileStream);
            new FileExtensionContentTypeProvider().TryGetContentType("test.png", out var contentType);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            using var formData = new MultipartFormDataContent();
            formData.Add(fileContent, "file", "test.png");

            var res = await httpClient.PutAsync("put_cover/2", formData);

        }
    }
}
