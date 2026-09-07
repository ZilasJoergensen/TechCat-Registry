using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TechCatRegistry.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddData(builder.Configuration);

// https://github.com/scalar/scalar/issues/6264#issuecomment-3077190454
// Fandt ud af at bygge options på scalar fra det her github issue
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info = new Microsoft.OpenApi.OpenApiInfo
        {
            Title = "TechCat Registry API Reference",
            Version = "v1",
            Description = "API Referencen for Zilas' svendeprøve projekt",
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TechCatDbContext>();
    try
    {
        logger.LogInformation("Connecting to database...");
        context.Database.Migrate();
        logger.LogInformation("Database ready.");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Failed to connect to database. Check connection string and that SQL Server is running.");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
