using BookFlix.Web.Mapper_Interfaces;
using BookFlix.Web.Mappers;

namespace BookFlix.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {

            // Add CORS service
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddLogging(logging =>
            {
                logging.AddConsole();
            });


            services.AddControllers();
            services.AddSwaggerGen();

            services.AddScoped<IBookMapper, BookMapper>();

            return services;
        }
    }
}
