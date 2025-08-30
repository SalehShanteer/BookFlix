//using BookFlix.Core.Abstractions;
//using BookFlix.Core.Models;
//using BookFlix.Core.Repositories;
//using BookFlix.Core.Service_Interfaces;
//using BookFlix.Core.Services;
//using BookFlix.Core.Services.Validation;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Moq;

//namespace BookFlix.Tests.Unit_Tests.Services
//{
//    public class LocalFileServiceTests
//    {
//        private readonly Mock<IBookRepository> _bookRepositoryMock;
//        private readonly Mock<IConfiguration> _configurationMock;
//        private readonly Mock<ITransaction> _transactionMock;
//        private readonly Mock<ILogger<LocalFileService>> _loggerMock;
//        private readonly IFileService _fileService;
//        private const string BookDirectory = @"C:\Users\saleh\Books\";

//        public LocalFileServiceTests()
//        {
//            _bookRepositoryMock = new Mock<IBookRepository>();
//            _configurationMock = new Mock<IConfiguration>();
//            _transactionMock = new Mock<ITransaction>();
//            _loggerMock = new Mock<ILogger<LocalFileService>>();
//            _configurationMock.Setup(c => c.GetSection("BookDirectory").Value).Returns(BookDirectory);
//            _fileService = new LocalFileService(_bookRepositoryMock.Object, _configurationMock.Object, _loggerMock.Object);
//        }

//        [Fact]
//        public async Task UploadFileAsync_NullFile_ReturnsBadRequest()
//        {
//            // Arrange
//            int bookId = 1;

//            // Act
//            var result = await _fileService.UploadFileAsync(bookId, null);

//            // Assert
//            Assert.False(result.IsValid);
//            Assert.Equal(enStatusCode.BadRequest, result.StatusCode);
//            Assert.Equal("File is null or empty.", result.Errors.Single());
//            _bookRepositoryMock.Verify(r => r.GetFileLocationAsync(It.IsAny<int>()), Times.Never);
//            _loggerMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task UploadFileAsync_NonPdfFile_ReturnsBadRequest()
//        {
//            // Arrange
//            int bookId = 1;
//            var fileMock = SetupFileMock("test.txt", "text/plain", 1024);

//            // Act
//            var result = await _fileService.UploadFileAsync(bookId, fileMock);

//            // Assert
//            Assert.False(result.IsValid);
//            Assert.Equal(enStatusCode.BadRequest, result.StatusCode);
//            Assert.Equal("Only PDF files are allowed.", result.Errors.Single());
//            _bookRepositoryMock.Verify(r => r.GetFileLocationAsync(It.IsAny<int>()), Times.Never);
//            _loggerMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task UploadFileAsync_Over100MbFile_ReturnsBadRequest()
//        {
//            // Arrange
//            int bookId = 1;
//            var fileMock = SetupFileMock("test.pdf", "application/pdf", 100 * 1024 * 1024 + 1);

//            // Act
//            var result = await _fileService.UploadFileAsync(bookId, fileMock);

//            // Assert
//            Assert.False(result.IsValid);
//            Assert.Equal(enStatusCode.BadRequest, result.StatusCode);
//            Assert.Equal("File size exceeds 100MB limit.", result.Errors.Single());
//            _bookRepositoryMock.Verify(r => r.GetFileLocationAsync(It.IsAny<int>()), Times.Never);
//            _loggerMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task UploadFileAsync_BookNotFound_ReturnsNotFound()
//        {
//            // Arrange
//            int bookId = 1;
//            var fileMock = SetupFileMock("test.pdf", "application/pdf", 1024);
//            _bookRepositoryMock.Setup(r => r.GetFileLocationAsync(bookId)).ReturnsAsync((Book?)null);

//            // Act
//            var result = await _fileService.UploadFileAsync(bookId, fileMock);

//            // Assert
//            Assert.False(result.IsValid);
//            Assert.Equal(enStatusCode.NotFound, result.StatusCode);
//            Assert.Equal($"Book with ID {bookId} not found.", result.Errors.Single());
//            _bookRepositoryMock.Verify(r => r.GetFileLocationAsync(bookId), Times.Once());
//            _bookRepositoryMock.Verify(r => r.BeginTransactionAsync(), Times.Never());
//            _loggerMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task UploadFileAsync_Success_ReturnsValidResult()
//        {
//            // Arrange
//            const int BookId = 1;
//            var fileMock = SetupFileMock("test.pdf", "application/pdf", 1024);
//            var book = new Book { Id = BookId, FileLocation = string.Empty };
//            _bookRepositoryMock.Setup(r => r.GetFileLocationAsync(BookId)).ReturnsAsync(book);
//            _bookRepositoryMock.Setup(r => r.UpdateFileLocationAsync(BookId, It.IsAny<string>())).Returns(Task.CompletedTask);
//            _bookRepositoryMock.Setup(r => r.BeginTransactionAsync()).ReturnsAsync(_transactionMock.Object);
//            _transactionMock.Setup(t => t.CommitAsync()).Returns(Task.CompletedTask);

//            // Act
//            var result = await _fileService.UploadFileAsync(BookId, fileMock);

//            // Assert
//            Assert.True(result.IsValid);
//            Assert.Empty(result.Errors);
//            Assert.NotNull(result.FileLocation);
//            Assert.Matches(@"^C:\\Users\\saleh\\Books\\[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}\.pdf$", result.FileLocation);
//            _bookRepositoryMock.Verify(r => r.GetFileLocationAsync(BookId), Times.Once());
//            _bookRepositoryMock.Verify(r => r.UpdateFileLocationAsync(BookId, It.Is<string>(s => s.EndsWith(".pdf"))), Times.Once());
//            _bookRepositoryMock.Verify(r => r.BeginTransactionAsync(), Times.Once());
//            _transactionMock.Verify(t => t.CommitAsync(), Times.Once());
//            _loggerMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task UploadFileAsync_IOException_ReturnsInternalServerError()
//        {
//            // Arrange
//            const int BookId = 1;
//            var fileMock = SetupFileMock("test.pdf", "application/pdf", 1024, throwsIOException: true);
//            var book = new Book { Id = BookId, FileLocation = string.Empty };
//            _bookRepositoryMock.Setup(r => r.GetFileLocationAsync(BookId)).ReturnsAsync(book);
//            _bookRepositoryMock.Setup(r => r.BeginTransactionAsync()).ReturnsAsync(_transactionMock.Object);
//            _transactionMock.Setup(t => t.RollbackAsync()).Returns(Task.CompletedTask);

//            // Act
//            var result = await _fileService.UploadFileAsync(BookId, fileMock);

//            // Assert
//            Assert.False(result.IsValid);
//            Assert.Equal(enStatusCode.InternalServerError, result.StatusCode);
//            Assert.Equal("Failed to save file due to a storage error.", result.Errors.Single());
//            _bookRepositoryMock.Verify(r => r.GetFileLocationAsync(BookId), Times.Once());
//            _bookRepositoryMock.Verify(r => r.UpdateFileLocationAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never());
//            _bookRepositoryMock.Verify(r => r.BeginTransactionAsync(), Times.Once());
//            _transactionMock.Verify(t => t.RollbackAsync(), Times.Once());
//            VerifyLog(LogLevel.Error, "IO error uploading file for book ID");
//        }

//        [Fact]
//        public async Task UploadFileAsync_GeneralException_ReturnsInternalServerError()
//        {
//            // Arrange
//            const int BookId = 1;
//            var fileMock = SetupFileMock("test.pdf", "application/pdf", 1024);
//            var book = new Book { Id = BookId, FileLocation = string.Empty };
//            _bookRepositoryMock.Setup(r => r.GetFileLocationAsync(BookId)).ReturnsAsync(book);
//            _bookRepositoryMock.Setup(r => r.UpdateFileLocationAsync(BookId, It.IsAny<string>())).ThrowsAsync(new Exception("Database error"));
//            _bookRepositoryMock.Setup(r => r.BeginTransactionAsync()).ReturnsAsync(_transactionMock.Object);
//            _transactionMock.Setup(t => t.RollbackAsync()).Returns(Task.CompletedTask);

//            // Act
//            var result = await _fileService.UploadFileAsync(BookId, fileMock);

//            // Assert
//            Assert.False(result.IsValid);
//            Assert.Equal(enStatusCode.InternalServerError, result.StatusCode);
//            Assert.Equal("An unexpected error occurred while uploading the file.", result.Errors.Single());
//            _bookRepositoryMock.Verify(r => r.GetFileLocationAsync(BookId), Times.Once());
//            _bookRepositoryMock.Verify(r => r.UpdateFileLocationAsync(BookId, It.IsAny<string>()), Times.Once());
//            _bookRepositoryMock.Verify(r => r.BeginTransactionAsync(), Times.Once());
//            _transactionMock.Verify(t => t.RollbackAsync(), Times.Once());
//            VerifyLog(LogLevel.Error, "Unexpected error uploading file for book ID");
//        }

//        private IFormFile SetupFileMock(string fileName, string contentType, long length, bool throwsIOException = false)
//        {
//            var fileMock = new Mock<IFormFile>();
//            fileMock.Setup(f => f.FileName).Returns(fileName);
//            fileMock.Setup(f => f.Length).Returns(length);
//            fileMock.Setup(f => f.ContentType).Returns(contentType);
//            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
//                .Returns((Stream target, CancellationToken token) => throwsIOException
//                    ? throw new IOException("Disk full")
//                    : Task.CompletedTask);
//            return fileMock.Object;
//        }

//        private void VerifyLog(LogLevel level, string message)
//        {
//            _loggerMock.Verify(l => l.Log(
//                level,
//                It.IsAny<EventId>(),
//                It.Is<It.IsAnyType>(v => v.ToString()!.Contains(message)),
//                It.IsAny<Exception>(),
//                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once());
//        }
//    }
//}