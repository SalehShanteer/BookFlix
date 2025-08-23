using BookFlix.Core.Dtos.Book;
using BookFlix.Core.Mappings;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.AspNetCore.Mvc;

namespace BookFlix.Web.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly IBookRepository _bookRepository;
        private readonly IBookService _bookService;
        private readonly IBookMappings _bookMappings;
        private readonly IFileService _fileService;

        public BooksController(IBookRepository bookRepository, IBookService bookService, IBookMappings bookMappings, IFileService fileService)
        {
            _bookRepository = bookRepository;
            _bookService = bookService;
            _bookMappings = bookMappings;
            _fileService = fileService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> GetBookByIdAsync(int id)
        {
            if (id < 1) return BadRequest("ID must be greater than 0.");

            var book = await _bookRepository.GetByIdAsync(id);

            if (book is null) return NotFound($"Book with ID = {id} not found!");

            BookDto bookDto = _bookMappings.ToBookDto(book);
            return Ok(bookDto);
        }

        [HttpPut("{id}/Upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> UploadBookAsync(int id, IFormFile file)
        {
            if (id < 1) return BadRequest("ID must be greater than 0.");

            var validationResult = await _fileService.UploadFileAsync(id, file);

            if (!validationResult.IsValid)
            {
                if (validationResult.StatusCode == enStatusCode.BadRequest) return BadRequest(validationResult.Errors);
                if (validationResult.StatusCode == enStatusCode.NotFound) return NotFound(validationResult.Errors);
                if (validationResult.StatusCode == enStatusCode.InternalServerError) return StatusCode(StatusCodes.Status500InternalServerError, validationResult.Errors);
            }
            return Ok(new { FileUrl = $"/books/{Path.GetFileName(validationResult.FileLocation)}" });
        }


        /// <summary>
        /// Adds a new book to the system.
        /// </summary>
        /// <param name="createBookDto">The book data to create.</param>
        /// <returns>A <see cref="BookDto"/> representing the created book, or an error response.</returns>
        /// <response code="201">Book created successfully.</response>
        /// <response code="400">Invalid book data provided.</response>
        /// <response code="500">Server error occurred.</response>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookDto>> AddBookAsync(BookInputDto createBookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validationResult = await _bookService.ValidateCreateBookDtoAsync(createBookDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                var book = await _bookMappings.ToBook(createBookDto);
                var addedBook = await _bookRepository.AddAsync(book);
                var bookDto = _bookMappings.ToBookDto(addedBook);
                return CreatedAtAction("GetBookById", new { id = bookDto.Id }, bookDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding book: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing book by ID.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="updateBookDto">The updated book data.</param>
        /// <returns>The updated <see cref="BookDto"/> or an error response.</returns>
        /// <response code="200">Book updated successfully.</response>
        /// <response code="400">Invalid book data or ID provided.</response>
        /// <response code="404">Book not found.</response>
        /// <response code="500">Server error occurred.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> UpdateBookAsync(int id, BookInputDto updateBookDto)
        {
            if (id < 1) return BadRequest("ID must be greater than 0.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isExists = await _bookRepository.IsExistById(id);

            if (!isExists) return BadRequest($"Book with ID = {id} does not exist.");

            var validationResult = await _bookService.ValidateCreateBookDtoAsync(updateBookDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                var book = await _bookMappings.ToBook(updateBookDto);
                book.Id = id;
                var updatedBook = await _bookRepository.UpdateAsync(book);
                var bookDto = _bookMappings.ToBookDto(updatedBook);
                return Ok(bookDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding book: {ex.Message}");
            }
        }
    }
}
