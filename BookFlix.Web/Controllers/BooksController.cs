using BookFlix.Core.Dtos.Book;
using BookFlix.Core.Mappings;
using BookFlix.Core.Repositories;
using BookFlix.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookFlix.Web.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly IBookRepository _bookRepository;
        private readonly BookService _bookService;

        public BooksController(IBookRepository bookRepository, BookService bookService)
        {
            _bookRepository = bookRepository;
            _bookService = bookService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> GetBookByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("ID must be greater than 0.");
            }
            var book = await _bookRepository.GetByIdAsync(id);

            if (book is null)
            {
                return NotFound($"Book with ID = {id} not found!");
            }

            BookDto bookDto = BookMappings.ToBookDto(book);
            return Ok(bookDto);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookDto>> AddBookAsync(CreateBookDto createBookDto)
        {
            if (createBookDto is null)
            {
                return BadRequest("CreateBookDto cannot be null.");
            }
            var validationResult = await _bookService.ValidateCreateBookDtoAsync(createBookDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                var book = BookMappings.ToBook(createBookDto);
                var addedBook = await _bookRepository.AddAsync(book);
                var bookDto = BookMappings.ToBookDto(addedBook);
                return CreatedAtAction(nameof(GetBookByIdAsync), new { id = bookDto.Id }, bookDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding book: {ex.Message}");
            }

        }
    }
}
