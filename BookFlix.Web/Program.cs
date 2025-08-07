using BookFlix.Infrastructure.Data;
using BookFlix.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

Dependencies.Configure(builder.Services);


// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


builder.Services.AddDbContext<AppDbContext>((options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    AppDbContextConfiguration.Configure((DbContextOptionsBuilder<AppDbContext>)options, connectionString);
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookFlix API V1"));
}

// Serve PDFs from BookDirectory in appsettings.json
var bookDirectory = builder.Configuration.GetValue<string>("BookDirectory")
    ?? throw new InvalidOperationException("BookDirectory not configured in appsettings.json.");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(bookDirectory),
    RequestPath = "/books"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
