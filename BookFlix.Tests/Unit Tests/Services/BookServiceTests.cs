//using BookFlix.Core.Repositories;
//using BookFlix.Core.Service_Interfaces;
//using BookFlix.Core.Services;
//using Microsoft.Extensions.Logging;
//using Moq;

//namespace BookFlix.Tests.Unit_Tests.Services
//{
//    public class BookServiceTests
//    {
//        private readonly Mock<IBookRepository> _bookRepositoryMock;
//        private readonly Mock<ILogger<BookService>> _loggerMock;
//        private readonly IBookService _bookService;

//        public BookServiceTests()
//        {
//            _bookRepositoryMock = new Mock<IBookRepository>();
//            _loggerMock = new Mock<ILogger<BookService>>();
//            _bookService = new BookService(_bookRepositoryMock.Object, _loggerMock.Object);
//        }

//        [Fact]
//        public async Task ValidateCreateBookDtoAsync_NullDto_ReturnsError()
//        {
//            // Arrange
//            BookInputDto? createBookDto = null;

//            // Act
//            var result = await _bookService.ValidateCreateBookDtoAsync(createBookDto);

//            // Assert
//            Assert.False(result.IsValid);
//            Assert.Equal("Book input cannot be null.", result.Errors.Single());
//            _bookRepositoryMock.Verify(r => r.IsExistByIsbnAsync(It.IsAny<string>()), Times.Never());
//            VerifyLog(LogLevel.Error, "BookInputDto validation failed: Input is null.");
//        }

//        [Fact]
//        public async Task ValidateCreateBookDtoAsync_FuturePublicationDate_ReturnsError()
//        {
//            // Arrange
//            var createBookDto = new BookInputDto { PublicationDate = DateTime.Now.AddDays(5), ISBN = "12345678910" };
//            _bookRepositoryMock.Setup(b => b.IsExistByISBNAsync(createBookDto.ISBN)).ReturnsAsync(false);

//            // Act
//            var result = await _bookService.ValidateCreateBookDtoAsync(createBookDto);

//            // Assert
//            Assert.False(result.IsValid);
//            Assert.Equal("The publication date should not be in the future", result.Errors.Single());
//            _bookRepositoryMock.Verify(b => b.IsExistByISBNAsync(createBookDto.ISBN), Times.Once());
//            VerifyLog(LogLevel.Warning, "Publication date validation failed");
//            VerifyLog(LogLevel.Error, "BookInputDto validation failed with 1 errors.");
//        }

//        [Fact]
//        public async Task ValidateCreateBookDtoAsync_ExistingISBN_ReturnsError()
//        {
//            // Arrange
//            string isbn = "12345678910";
//            var createBookDto = new BookInputDto { ISBN = isbn, PublicationDate = DateTime.Now.AddDays(-1) };
//            _bookRepositoryMock.Setup(b => b.IsExistByIsbnAsync(isbn)).ReturnsAsync(true);

//            // Act
//            var result = await _bookService.ValidateCreateBookDtoAsync(createBookDto);

//            // Assert
//            Assert.False(result.IsValid);
//            Assert.Equal($"A book with ISBN {isbn} already exists.", result.Errors.Single());
//            _bookRepositoryMock.Verify(b => b.IsExistByIsbnAsync(isbn), Times.Once());
//            VerifyLog(LogLevel.Warning, $"ISBN validation failed: A book with ISBN {isbn} already exists.");
//            VerifyLog(LogLevel.Error, "BookInputDto validation failed with 1 errors.");
//        }

//        [Fact]
//        public async Task ValidateCreateBookDtoAsync_NullISBN_ReturnsError()
//        {
//            // Arrange
//            var createBookDto = new BookInputDto { PublicationDate = DateTime.Now.AddDays(-1), ISBN = null };

//            // Act
//            var result = await _bookService.ValidateCreateBookDtoAsync(createBookDto);

//            // Assert
//            Assert.False(result.IsValid);
//            Assert.Equal("ISBN cannot be null or empty.", result.Errors.Single());
//            _bookRepositoryMock.Verify(b => b.IsExistByIsbnAsync(It.IsAny<string>()), Times.Once());
//            VerifyLog(LogLevel.Warning, "ISBN validation failed: ISBN is null or empty.");
//            VerifyLog(LogLevel.Error, "BookInputDto validation failed with 1 errors.");
//        }

//        [Fact]
//        public async Task ValidateCreateBookDtoAsync_ValidDto_ReturnsNoError()
//        {
//            // Arrange
//            var createBookDto = new BookInputDto { PublicationDate = new DateTime(2005, 4, 8), ISBN = "12345678910" };
//            _bookRepositoryMock.Setup(b => b.IsExistByISBNAsync(createBookDto.ISBN)).ReturnsAsync(false);

//            // Act
//            var result = await _bookService.ValidateCreateBookDtoAsync(createBookDto);

//            // Assert
//            Assert.True(result.IsValid);
//            Assert.Empty(result.Errors);
//            _bookRepositoryMock.Verify(b => b.IsExistByISBNAsync(createBookDto.ISBN), Times.Once());
//            VerifyLog(LogLevel.Information, $"Starting validation for BookInputDto with ISBN {createBookDto.ISBN}");
//            VerifyLog(LogLevel.Information, $"ISBN {createBookDto.ISBN} is valid and not previously used.");
//            VerifyLog(LogLevel.Information, $"BookInputDto validation succeeded for ISBN {createBookDto.ISBN}");
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
