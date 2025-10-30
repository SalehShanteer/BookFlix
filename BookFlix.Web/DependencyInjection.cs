using BookFlix.Web.Mapper_Interfaces;
using BookFlix.Web.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

        private static void SwaggerConfiguration(IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "BookFlix API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
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
            SwaggerConfiguration(services);

            services.AddScoped<IBookMapper, BookMapper>();
            services.AddScoped<IUserLogMapper, UserLogMapper>();
            services.AddScoped<IUserMapper, UserMapper>();

            return services;
        }
    }
}
