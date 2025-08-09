using BookFlix.Core.Services.Validation.Book;
using Microsoft.AspNetCore.Http;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IFileService
    {
        Task<UpdateBookValidationResult> UploadFileAsync(int bookId, IFormFile? file);
    }
}
