using StorageAPI.AppConfig.Attributes;
using StorageAPI.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace StorageAPI.Endpoints
{
    public sealed class CoverHandlers
    {

        public const long MAX_UPLOAD_IMAGE_SIZE_FULL = 2 * 1024 * 1024; // 2MB
        public const long MAX_UPLOAD_IMAGE_SIZE_150 = 200 * 1024; // 200KB
        private const string COVER_EXTENSION = ".png";

        private static readonly string[] ALLOWED_MIME = ["image/x-png", "image/png"];


        internal static IResult DeleteCover(HttpContext context, string id)
        {
            if (!ValidId(id, out var badResult))
                return badResult ?? throw new UnreachableException();

            try
            {
                File.Delete("/cover/" + id);
            }
            catch (Exception) { }

            return Results.Ok();
        }

        [ApiKeyAuthorization]
        internal static async Task<IResult> PutCover(string id, IFormFile full, IFormFile resized_150, ILogger<CoverHandlers> logger)
        {
            if (!ValidId(id, out var badResult))
                return badResult ?? throw new UnreachableException();

            if (full.SizeGreaterThan(MAX_UPLOAD_IMAGE_SIZE_FULL))
                return Results.BadRequest($"'full' cover file should be under {MAX_UPLOAD_IMAGE_SIZE_FULL}KB.");
            if (resized_150.SizeGreaterThan(MAX_UPLOAD_IMAGE_SIZE_150))
                return Results.BadRequest($"'resized_150' cover file should be under {MAX_UPLOAD_IMAGE_SIZE_150}KB.");

            if (!full.EndsWithExtension(COVER_EXTENSION))
                return Results.BadRequest($"Expected 'full' file name to end with {COVER_EXTENSION}.");
            if (!resized_150.EndsWithExtension(COVER_EXTENSION))
                return Results.BadRequest($"Expected 'resized_150' file name to end with {COVER_EXTENSION}.");

            if (!full.HasAnyMimeType(ALLOWED_MIME))
                return Results.BadRequest("full: Wrong mime type; Expected image/x-png or image/png.");
            if (!resized_150.HasAnyMimeType(ALLOWED_MIME))
                return Results.BadRequest("resized_150: Wrong mime type; Expected image/x-png or image/png.");

            try
            {
                using var fullFileStream = new FileStream($"/storage/cover/{id}-full{COVER_EXTENSION}", FileMode.Create);
                using var resized_150_FileStream = new FileStream($"/storage/cover/{id}-150{COVER_EXTENSION}", FileMode.Create);
                await full.CopyToAsync(fullFileStream);
                await resized_150.CopyToAsync(resized_150_FileStream);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Could not save cover for media with id: {mediaId}.", id);
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Results.Ok();
        }


        /// <summary>
        /// Returns whether or not the given id is valid. Must have only alphanumeric characters or -
        /// </summary>
        /// <param name="id"> the id to check </param>
        /// <param name="badResult"> Has a value only if the id is not valid </param>
        private static bool ValidId(string id, out IResult? badResult)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                badResult = Results.BadRequest("id parameter must have a value");
                return false;
            }
            if (id.Any(c => !char.IsLetterOrDigit(c) && c != '-'))
            {
                badResult = Results.BadRequest("id parameter must contain only alphanumeric characters or -");
                return false;
            }

            badResult = null;
            return true;
        }
    }
}
