using BookFlix.Core.Dtos.Book;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services;
using Moq;

namespace BookFlix.Tests.Unit_Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly IBookService _bookService;
        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookService = new BookService(_bookRepositoryMock.Object);
        }

        [Fact]
        public async Task ValidateCreateBookDtoAsync_NullDto_ReturnsError()
        {
            // Arrange
            BookInputDto? createBookDto = null;

            // Act
            var result = await _bookService.ValidateCreateBookDtoAsync(createBookDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Book input cannot be null.", result.Errors);
            Assert.Single(result.Errors);
            _bookRepositoryMock.Verify(r => r.GetByISBNAsync(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task ValidateCreateBookDtoAsync_FuturePublicationDate_ReturnsError()
        {
            // Arrange
            var createBookDto = new BookInputDto { PublicationDate = DateTime.Now.AddDays(5), ISBN = "12345678910" };

            // Act
            var result = await _bookService.ValidateCreateBookDtoAsync(createBookDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("The publication date should not be in the future", result.Errors);
            Assert.Single(result.Errors);
        }

        [Fact]
        public async Task ValidateCreateBookDtoAsync_ExistingISBN_ReturnsError()
        {
            // Arrange
            string isbn = "12345678910";
            _bookRepositoryMock.Setup(b => b.IsExistByISBNAsync(isbn)).ReturnsAsync(true);
            var createBookDto = new BookInputDto { ISBN = isbn };

            // Act 
            var result = await _bookService.ValidateCreateBookDtoAsync(createBookDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains($"A book with ISBN {isbn} already exists.", result.Errors);
            Assert.Single(result.Errors);
            _bookRepositoryMock.Verify(b => b.IsExistByISBNAsync(isbn), Times.Once());
        }

        [Fact]
        public async Task ValidateCreateBookDtoAsync_NullISBN_ReturnsNoError()
        {
            // Arrange
            var createBookDto = new BookInputDto
            {
                PublicationDate = DateTime.Now.AddDays(-1),
                ISBN = null
            };

            // Act
            var result = await _bookService.ValidateCreateBookDtoAsync(createBookDto);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
            _bookRepositoryMock.Verify(b => b.IsExistByISBNAsync(createBookDto.ISBN), Times.Once());
        }

        [Fact]
        public async Task ValidateCreateBookDtoAsync_ValidDto_ReturnsNoError()
        {
            // Arrange
            var createBookDto = new BookInputDto
            {
                PublicationDate = new DateTime(2005, 4, 8),
                ISBN = "12345678910"
            };

            // Act 
            var result = await _bookService.ValidateCreateBookDtoAsync(createBookDto);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
            _bookRepositoryMock.Verify(b => b.IsExistByISBNAsync(createBookDto.ISBN), Times.Once());
        }
    }
}
