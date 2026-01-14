using Microsoft.AspNetCore.Http;

namespace MiniApp.BLL.Abstractions.Externals
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file,string folder);
        Task RemoveFileAsync(string fileUrl);
        Task<string> UpdateFileAsync(IFormFile file, string filePath,string folder);
    }
}
