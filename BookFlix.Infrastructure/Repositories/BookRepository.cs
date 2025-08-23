using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookFlix.Infrastructure.Repositories
{
    public class BookRepository : TransactionRepository, IBookRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(AppDbContext context, ILogger<BookRepository> logger) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Book> AddAsync(Book entity)
        {
            try
            {
                await _context.Books.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to add book with ISBN {ISBN} due to database error.", entity.ISBN);
                throw new Exception("Failed to add book due to database error.", ex);
            }
        }

        public async Task<IReadOnlyCollection<Book>> GetAllAsync()
        {
            return await _context.Books
                .AsSplitQuery()
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Book>> GetByAuthorIdAsync(int authorId)
        {
            return await _context.Books
                .AsNoTracking()
                .Where(b => b.Authors.Any(a => a.Id == authorId))
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .AsSplitQuery()
                .AsNoTracking()
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> UpdateAsync(Book entity)
        {
            try
            {
                _context.Books.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to update book with ID {BookId} due to database error.", entity.Id);
                throw new Exception("Failed to update book due to database error.", ex);
            }
        }

        public async Task UpdateFileLocationAsync(int id, string fileLocation)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                book.FileLocation = fileLocation;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book is null)
                {
                    return false;
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to delete book with ID {BookId} due to database error.", id);
                throw new Exception("Failed to delete book due to database error.", ex);
            }
        }

        public async Task<Book?> GetByISBNAsync(string? isbn)
        {
            return await _context.Books
                .AsSplitQuery()
                .AsNoTracking()
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<bool> IsExistByISBNAsync(string? isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return false;
            }

            return await _context.Books.AsNoTracking().AnyAsync(b => b.ISBN == isbn);
        }

        public async Task<bool> IsExistById(int id)
        {
            if (id < 1)
            {
                return false;
            }

            return await _context.Books.AsNoTracking().AnyAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByIdForUpdateFileLocationAsync(int id)
        {
            return await _context.Books
                .AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new Book { Id = b.Id, FileLocation = b.FileLocation })
                .FirstOrDefaultAsync();
        }
    }
}