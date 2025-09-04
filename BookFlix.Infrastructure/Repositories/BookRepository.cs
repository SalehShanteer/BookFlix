using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    public class BookRepository : TransactionRepository, IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Book> AddAsync(Book entity)
        {
            await _context.Books.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IReadOnlyCollection<Book>> GetAllAsync()
        => await _context.Books
                .AsSplitQuery()
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IReadOnlyCollection<Book>> GetByAuthorIdAsync(int authorId)
        => await _context.Books
                .AsNoTracking()
                .Where(b => b.Authors.Any(a => a.Id == authorId))
                .ToListAsync();

        public async Task<Book?> GetByIdAsync(int id)
        => await _context.Books
                .AsSplitQuery()
                .AsNoTracking()
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<Book?> GetByIdForUpdateAsync(int id)
             => await _context.Books
                .AsSplitQuery()
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<Book> UpdateAsync(Book entity)
        {
            _context.Books.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateFileLocationAsync(int id, string fileLocation)
        {
            var book = await _context.Books.FindAsync(id);
            if (book is null) return false;

            book.FileLocation = fileLocation;
            book.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book is null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        => await _context.Books
                .AsSplitQuery()
                .AsNoTracking()
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.ISBN == isbn);

        public async Task<bool> IsExistByIsbnAsync(string isbn) => await _context.Books.AsNoTracking().AnyAsync(b => b.ISBN == isbn);

        public async Task<bool> IsExistByIsbnAsync(int id, string isbn) => await _context.Books.AsNoTracking().AnyAsync(b => b.ISBN == isbn && b.Id != id);


        public async Task<bool> IsExistById(int id) => await _context.Books.AsNoTracking().AnyAsync(b => b.Id == id);

        public async Task<(string? FileLocation, bool IsBookExist)> GetFileLocationAsync(int id)
        {
            var result = await _context.Books
                .AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new { FileLocation = b.FileLocation })
                .FirstOrDefaultAsync();

            if (result is null)
                return (null, false);

            return (result.FileLocation, true);
        }


    }
}