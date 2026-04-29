
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductService API v1"));
}

app.UseHttpsRedirection();

// Global Exception Handling
app.UseMiddleware<ProductService.API.Middlewares.ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine($"Current environment: {app.Environment.EnvironmentName}");
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    dbContext.Database.Migrate();
    Console.WriteLine("Database migrated successfully.");
}

app.Run();
