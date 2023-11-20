using StorageAPI.AppConfig.Attributes;
using System.Diagnostics;

namespace StorageAPI.Endpoints
{
    public static class CoverHandlers
    {

        public const long MAX_UPLOAD_IMAGE_SIZE = 200 * 1024; // 200KB


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
        internal static async Task<IResult> PutCover(string id, IFormFile file)
        {
            if (ValidId(id, out var badResult))
                return badResult ?? throw new UnreachableException();

            var fileSizeInKb = file.Length;
            if (fileSizeInKb > MAX_UPLOAD_IMAGE_SIZE)
                return Results.BadRequest($"File should be under {MAX_UPLOAD_IMAGE_SIZE}KB.");

            var fileExtension = Path.GetExtension(file.FileName)?.ToLower();
            if (fileExtension != ".png")
                return Results.BadRequest("Expected .png file.");

            var mimeType = file.ContentType.ToLower();
            if (mimeType != "image/x-png" &&
                mimeType != "image/png")
                return Results.BadRequest("Wrong mime type; Expected image/x-png or image/png.");


            using var toFileStream = new FileStream("/cover/id" + fileExtension, FileMode.Create);
            await file.CopyToAsync(toFileStream);

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
