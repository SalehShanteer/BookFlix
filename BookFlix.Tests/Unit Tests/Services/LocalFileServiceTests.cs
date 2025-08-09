using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services;
using BookFlix.Core.Services.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BookFlix.Tests.Unit_Tests.Services
{
    public class LocalFileServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly IFileService _fileService;

        public LocalFileServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _configurationMock = new Mock<IConfiguration>();
            var configSection = new Mock<IConfigurationSection>();
            configSection.Setup(c => c.Value).Returns(@"C:\Users\saleh\Books");
            _configurationMock.Setup(c => c.GetSection("BookDirectory")).Returns(configSection.Object);
            _fileService = new LocalFileService(_bookRepositoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task UploadFileAsync_NullFile_ReturnsError()
        {
            // Arrange
            int bookId = 1;
            IFormFile? file = null;

            // Act
            var result = await _fileService.UploadFileAsync(bookId, file);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(enStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("File is null or empty.", result.Errors);
            Assert.Single(result.Errors);
            _bookRepositoryMock.Verify(b => b.GetByIdForUpdateFileLocationAsync(bookId), Times.Never);
        }

        [Fact]
        public async Task UploadFileAsync_NonPdfFile_ReturnsBadRequest()
        {
            // Arrange
            int bookId = 1;
            var file = _CreateFormFileMock("test.txt", "text/plain", 1024);

            // Act
            var result = await _fileService.UploadFileAsync(bookId, file);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(enStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("Only PDF files are allowed.", result.Errors);
            Assert.Single(result.Errors);
            _bookRepositoryMock.Verify(b => b.GetByIdForUpdateFileLocationAsync(bookId), Times.Never);
        }

        [Fact]
        public async Task UploadFileAsync_Over100MbFile_ReturnsBadRequest()
        {
            // Arrange
            int bookId = 1;
            var file = _CreateFormFileMock("test.pdf", "application/pdf", 100 * 1024 * 1024 + 1);

            // Act
            var result = await _fileService.UploadFileAsync(bookId, file);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(enStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("File size exceeds 100MB limit.", result.Errors);
            Assert.Single(result.Errors);
            _bookRepositoryMock.Verify(b => b.GetByIdForUpdateFileLocationAsync(bookId), Times.Never);
        }

        [Fact]
        public async Task UploadFileAsync_NotFoundBook_ReturnsNotFound()
        {
            // Arrange
            int bookId = 1;
            var file = _CreateFormFileMock("test.pdf", "application/pdf", 1024);
            _bookRepositoryMock.Setup(b => b.GetByIdForUpdateFileLocationAsync(bookId)).ReturnsAsync((Book?)null);

            // Act
            var result = await _fileService.UploadFileAsync(bookId, file);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(enStatusCode.NotFound, result.StatusCode);
            Assert.Contains($"Book with ID {bookId} not found.", result.Errors);
            Assert.Single(result.Errors);
            _bookRepositoryMock.Verify(b => b.GetByIdForUpdateFileLocationAsync(bookId), Times.Once);
        }

        [Fact]
        public async Task UploadFileAsync_Success_ReturnsValidResult()
        {
            // Arrange
            int bookId = 1;
            var fileMock = _CreateFormFileMock("test.pdf", "application/pdf", 1024);
            var book = new Book { Id = bookId, FileLocation = string.Empty };
            _bookRepositoryMock.Setup(r => r.GetByIdForUpdateFileLocationAsync(bookId)).ReturnsAsync(book);
            _bookRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            // Act
            var result = await _fileService.UploadFileAsync(bookId, fileMock);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.FileLocation);
            Assert.EndsWith(".pdf", result.FileLocation);
            Assert.StartsWith(@"C:\Users\saleh\Books\", result.FileLocation);
            _bookRepositoryMock.Verify(r => r.GetByIdForUpdateFileLocationAsync(bookId), Times.Once());
            _bookRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Book>(b => b.Id == bookId && b.FileLocation!.EndsWith(".pdf"))), Times.Once());
        }

        [Fact]
        public async Task UploadFileAsync_IOException_ReturnsInternalServerError()
        {
            // Arrange
            int bookId = 1;
            var fileMock = _CreateFormFileMock("test.pdf", "application/pdf", 1024);
            var book = new Book { Id = bookId, FileLocation = string.Empty };
            _bookRepositoryMock.Setup(r => r.GetByIdForUpdateFileLocationAsync(bookId)).ReturnsAsync(book);
            _bookRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Book>())).ThrowsAsync(new IOException("Disk full"));

            // Act
            var result = await _fileService.UploadFileAsync(bookId, fileMock);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Failed to save file: Disk full", result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal(enStatusCode.InternalServerError, result.StatusCode);
            _bookRepositoryMock.Verify(r => r.GetByIdForUpdateFileLocationAsync(bookId), Times.Once());
            _bookRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Once());
        }


        private static IFormFile _CreateFormFileMock(string fileName, string contentType, long length)
        {
            var fileMock = new Mock<IFormFile>();
            var stream = new MemoryStream(new byte[length]);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(length);
            fileMock.Setup(f => f.ContentType).Returns(contentType);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream target, CancellationToken token) => stream.CopyToAsync(target, token));
            return fileMock.Object;
        }
    }
}
