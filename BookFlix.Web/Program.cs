using BookFlix.Core;
using BookFlix.Infrastructure;
using BookFlix.Web;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCore();
builder.Services.AddApiServices();

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


app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandlerFeature != null)
        {
            var error = new { message = exceptionHandlerFeature.Error.Message };
            await context.Response.WriteAsJsonAsync(error);
        }
    });
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
