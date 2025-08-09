using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{

    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
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
                throw new Exception("Failed to add book due to database error.", ex);
            }
        }

        public async Task<IReadOnlyCollection<Book>> GetAllAsync() => await _context.Books.Include(b => b.Authors).Include(b => b.Genres).AsNoTracking().ToListAsync();


        public async Task<IReadOnlyCollection<Book>> GetByAuthorIdAsync(int authorId)
        {
            return await _context.Books.AsNoTracking().Where(b => b.Authors.Any(a => a.Id == authorId)).ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
            => await _context.Books.AsNoTracking().Include(b => b.Authors).Include(b => b.Genres).FirstOrDefaultAsync(b => b.Id == id);



        public async Task UpdateAsync(Book entity)
        {
            try
            {

                _context.Books.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to update book due to database error.", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book is null) return false;

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }

            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to delete book due to database error.", ex);
            }

        }

        public async Task<Book?> GetByISBNAsync(string? isbn) => await _context.Books.AsNoTracking().Include(b => b.Authors).Include(b => b.Genres).FirstOrDefaultAsync(b => b.ISBN == isbn);

        public async Task<bool> IsExistByISBNAsync(string? isbn)
        {
            if (string.IsNullOrEmpty(isbn)) return false;

            return await _context.Books.AsNoTracking().AnyAsync(b => b.ISBN == isbn);
        }

        public async Task<Book?> GetByIdForUpdateFileLocationAsync(int id)
        {
            return await _context.Books.AsNoTracking().Where(b => b.Id == id).Select(b => new Book { Id = b.Id, FileLocation = b.FileLocation }).FirstOrDefaultAsync();
        }
    }
}
