namespace StorageAPI.Extensions
{
    public static class IFormFileExtensions
    {
        public static bool SizeGreaterThan(this  IFormFile file, long size)
        {
            return file.Length > size;
        }

        public static bool EndsWithExtension(this IFormFile file, string extension)
        {
            var fileExtension = Path.GetExtension(file.FileName)?.ToLower();
            return fileExtension == extension;
        }

        /// <summary>
        /// Check if file's mime is between provided mime types
        /// </summary>
        /// <param name="file">IFormFile to check</param>
        /// <param name="mime">Provieded mime tyeps. ex: ["image/png", "image/x-png"]</param>
        /// <returns>True if any of the provided mime matches the file's mime type</returns>
        public static bool HasAnyMimeType(this IFormFile file, params string[] mime)
        {
            return mime.Any(x => x.Equals(file.ContentType, StringComparison.OrdinalIgnoreCase));
        }
    }
}
