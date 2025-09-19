using BookFlix.Core.Service_Interfaces;
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooksAsync()
        {
            var books = await _bookService.GetAllBooksAsync();
            var bookDtos = _bookMapper.ToBookDtos(books.ToList());
            return Ok(bookDtos);
        }

        [HttpPut("{id}/Upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FileUploadResultDto>> UploadBookAsync(int id, IFormFile file)
        {
            if (id < 1) return BadRequest("ID must be greater than 0.");

            var validationResult = await _fileService.UploadFileAsync(id, file);

            if (!validationResult.IsValid)
            {
                return validationResult.ToActionResult<FileUploadResultDto>();
            }
            return Ok(new FileUploadResultDto { FileUrl = validationResult.FileLocation });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookDto>> AddBookAsync(BookCreateDto createBookDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var bookResult = await _bookMapper.ToBook(createBookDto);

            if (!bookResult.Result.IsValid) return NotFound(bookResult.Result.Errors);
            var book = bookResult.Book;

            var newBookResult = await _bookService.AddBookAsync(book);

            if (!newBookResult.result.IsValid) return BadRequest(newBookResult.result.Errors);
            book = newBookResult.book;
            var bookDto = _bookMapper.ToBookDto(book!);

            return CreatedAtAction("GetBookById", new { id = bookDto.Id }, bookDto);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> UpdateBookAsync(int id, BookUpdateDto bookUpdateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id < 1) return BadRequest("ID must be greater than 0.");

            bookUpdateDto.Id = id;

            var bookResult = await _bookMapper.ToBook(bookUpdateDto);
            if (!bookResult.Result.IsValid) return NotFound(bookResult.Result.Errors);

            var book = bookResult.Book;

            var validationResult = await _bookService.UpdateBookAsync(book);

            if (!validationResult.result.IsValid) return BadRequest(validationResult.result.Errors);

            book = validationResult.book;
            var bookDto = _bookMapper.ToBookDto(book!);
            return Ok(bookDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBookAsync(int id)
        {
            if (id < 1) return BadRequest("ID must be greater than 0.");

            var result = await _bookService.DeleteBookAsync(id);

            if (!result.IsValid)
            {
                return result.ToActionResult();
            }
            return NoContent();
        }

    }
}
