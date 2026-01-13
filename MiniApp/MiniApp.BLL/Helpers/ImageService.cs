using Microsoft.AspNetCore.Http;

namespace MiniApp.BLL.Helpers
{
    internal static class ImageService
    {
        public static bool IsImage(IFormFile file)
        {
            if (file is null) return true;

            var extensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            var mimeTypes = new[] { "image/jpeg", "image/png"};

            return extensions.Contains(ext) && mimeTypes.Contains(file.ContentType);
        }
        public static string ExtractKeyFromUrl(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            return uri.AbsolutePath.TrimStart('/');
        }
    }
}
