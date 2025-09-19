using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookFlix.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IFileService, LocalFileService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserLogService, UserLogService>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
