using BookFlix.Core.Services.Validation;
using Microsoft.AspNetCore.Http;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IFileService
    {
        Task<Result<string>> UploadFileAsync(Guid bookId, IFormFile file);
    }
}
