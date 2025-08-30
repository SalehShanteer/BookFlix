using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using BookFlix.Web.Dtos.Book;
using BookFlix.Web.Mapper_Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookFlix.Web.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly IBookService _bookService;
        private readonly IFileService _fileService;
        private readonly IBookMapper _bookMapper;

        public BooksController(IBookService bookService, IFileService fileService, IBookMapper bookMapper)
        {
            _bookService = bookService;
            _fileService = fileService;
            _bookMapper = bookMapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> GetBookByIdAsync(int id)
        {
            if (id < 1) return BadRequest("ID must be greater than 0.");

            var book = await _bookService.GetBookByIdAsync(id);

            if (book is null) return NotFound($"Book with ID = {id} not found!");

            BookDto bookDto = _bookMapper.ToBookDto(book);
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
            return Ok(new FileUploadResultDto { FileUrl = validationResult.FileLocation });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookDto>> AddBookAsync(BookCreateDto createBookDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var book = await _bookMapper.ToBook(createBookDto);
            var validationResult = await _bookService.AddBookAsync(book);

            if (!validationResult.result.IsValid) return BadRequest(validationResult.result.Errors);

            book = validationResult.book;
            var bookDto = _bookMapper.ToBookDto(book!);
            return CreatedAtAction("GetBookById", new { id = bookDto.Id }, bookDto);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> UpdateBookAsync(BookUpdateDto bookUpdateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (bookUpdateDto.Id < 1) return BadRequest("ID must be greater than 0.");

            var book = await _bookMapper.ToBook(bookUpdateDto);
            var validationResult = await _bookService.UpdateBookAsync(book);

            if (!validationResult.result.IsValid) return BadRequest(validationResult.result.Errors);

            book = validationResult.book;
            var bookDto = _bookMapper.ToBookDto(book!);
            return Ok(bookDto);

        }
    }
}
