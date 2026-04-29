
using Microsoft.EntityFrameworkCore;
using ProductService.Application;
using ProductService.Infrastructure;
using ProductService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Layer Dependencies
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// ─── Startup Migration ────────────────────────────────────────
// Migration her ortamda (development, staging, production) container
// ayağa kalkarken otomatik olarak uygulanır. CI pipeline'a gerek yok.
await ApplyMigrationsAsync(app);

// ─── Middleware Pipeline ──────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductService API v1"));
}

app.UseHttpsRedirection();
app.UseMiddleware<ProductService.API.Middlewares.ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();

// ─── Local Functions ──────────────────────────────────────────
static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying {Count} pending migration(s)...", pendingMigrations.Count());
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully.");
        }
        else
        {
            logger.LogInformation("Database is up to date. No migrations to apply.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying database migrations.");
        throw;
    }
}
