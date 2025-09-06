using BookFlix.Web.Mapper_Interfaces;
using BookFlix.Web.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookFlix.Web
{
    public static class DependencyInjection
    {


        private static byte[] GetJwtKey(IConfigurationSection jwtSettings)
        {
            if (jwtSettings is not null && jwtSettings["Key"] is not null)
            {
                return Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
            }

            throw new Exception("Error: JWT key not found");
        }

        private static void JwtConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");

            byte[] key = GetJwtKey(jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {

            JwtConfiguration(services, configuration);

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

            services.AddAuthorization();
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddScoped<IBookMapper, BookMapper>();
            services.AddScoped<IUserLogMapper, UserLogMapper>();

            return services;
        }
    }
}
