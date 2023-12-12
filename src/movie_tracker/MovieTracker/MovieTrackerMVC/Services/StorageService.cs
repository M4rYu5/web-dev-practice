using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net.Http;
using SkiaSharp;


namespace MovieTrackerMVC.Services
{

    public class StorageService
    {
        private record PngCovers(MemoryStream Full, MemoryStream Resized_150) : IDisposable
        {
            public void Dispose()
            {
                Full.Dispose(); 
                Resized_150.Dispose();
            }
        };


        private readonly HttpClient httpClient;


        public StorageService(HttpClient httpClient, IOptions<StorageOptions> appConfig)
        {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri(appConfig.Value.StorageAddress);
            this.httpClient.DefaultRequestHeaders.Add("API-KEY", appConfig.Value.StorageKey);
        }


        /// <summary>
        /// Resize, format and upload cover.
        /// </summary>
        /// <param name="cover">The IFormFile cover</param>
        /// <returns>Succeed status</returns>
        public async Task<bool> UploadCover(string mediaId, IFormFile cover, ILogger? logger)
        {
            try
            {
                using var st = cover.OpenReadStream();
                using var Covers = GeneratePNGs(st, 150, 150);

                using var fullContent = new StreamContent(Covers.Full);
                fullContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                using var resized_150Content = new StreamContent(Covers.Resized_150);
                resized_150Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

                using var formData = new MultipartFormDataContent
                {
                    { fullContent, "full", "cover.png" },
                    { resized_150Content, "resized_150", "cover.png" }
                };

                var storageResult = await httpClient.PutAsync($"put_cover/{mediaId}", formData);
                if (!storageResult.IsSuccessStatusCode)
                {
                    logger?.LogError("Got {StatusCode} code when trying to 'put_cover/{MediaId}'", storageResult.StatusCode, mediaId);
                    return false;
                }

            }
            catch (Exception e){ 
                logger?.LogError("Could not upload the cover for media id '{MediaId}', with Exception: {Exception}", mediaId, e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Generate the resized image (thubnail), and redraw the original, both in PNG format.
        /// </summary>
        /// <param name="cover">Image's stream</param>
        /// <param name="maxWidth">max widht, keeps the image ratio<Mparam>
        /// <param name="maxHeight">max height, keeps the image ratio</param>
        /// <returns>PNG MemoryStream for both, original and resized</returns>
        private static PngCovers GeneratePNGs(Stream cover, int maxWidth, int maxHeight)
        {
            // Load the image from the streams
            using var original = SKBitmap.Decode(cover);

            // Calculate the new dimensions while maintaining aspect ratio
            var ratio = Math.Min((float)maxWidth / original.Width, (float)maxHeight / original.Height);
            var newWidth = (int)(original.Width * ratio);
            var newHeight = (int)(original.Height * ratio);

            // Create the new bitmap and draw the original bitmap onto it
            var resized = new SKBitmap(newWidth, newHeight);
            using var canvas = new SKCanvas(resized);
            canvas.DrawBitmap(original, new SKRect(0, 0, newWidth, newHeight));

            // Convert the resized SKBitmap to a StreamContent
            var ms_r = new MemoryStream();
            resized.Encode(ms_r, SKEncodedImageFormat.Png, 0); // PNG types doesn't respect/support quality input
            ms_r.Seek(0, SeekOrigin.Begin);

            // Convert the original SKBitmap to a StreamContent
            var ms_o = new MemoryStream();
            original.Encode(ms_o, SKEncodedImageFormat.Png, 0); // PNG types doesn't respect/support quality input
            ms_o.Seek(0, SeekOrigin.Begin);

            return new PngCovers(Full: ms_o, Resized_150: ms_r);
        }
    }
}
