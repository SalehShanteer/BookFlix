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

        [HttpPost("{id}/Upload")]
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

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookDto>> AddBookAsync(BookInputDto createBookDto)
        {
            if (createBookDto is null)
            {
                return BadRequest("CreateBookDto cannot be null.");
            }

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

        //[HttpPut("Update")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<BookDto>> UpdateBookAsync(BookInputDto createBookDto)
        //{

        //}
    }
}
