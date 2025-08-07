using BookFlix.Core.Mappings;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services;
using BookFlix.Infrastructure.Repositories;

namespace BookFlix.Web.Helpers
{
    public class Dependencies
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IBookMappings, BookMappings>();
            services.AddScoped<IFileService, LocalFileService>();

        }
    }
}
