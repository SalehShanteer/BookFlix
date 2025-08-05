using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookFlix.Infrastructure.Data
{
    public static class AppDbContextConfiguration
    {
        public static void Configure(DbContextOptionsBuilder<AppDbContext> optionsBuilder, string? constr)
        {
            optionsBuilder.UseSqlServer(constr)
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
