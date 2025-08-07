using BookFlix.Core.Services;
using Microsoft.AspNetCore.Http;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IFileService
    {
        Task<ValidationResult> UploadFileAsync(int bookId, IFormFile file);
    }
}
