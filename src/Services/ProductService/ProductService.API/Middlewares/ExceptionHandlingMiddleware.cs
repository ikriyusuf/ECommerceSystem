using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ProductService.API.Middlewares;

/// <summary>
/// Tüm işlenmemiş exception'ları yakalar ve standart bir JSON hata yanıtı döndürür.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation hatası: {Errors}", ex.Errors);
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "İşlenmeyen bir hata oluştu.");
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = ex.Errors.Select(e => e.ErrorMessage).Distinct();

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Doğrulama Hatası",
            Detail = "Bir veya daha fazla doğrulama hatası oluştu.",
            Extensions = { ["errors"] = errors }
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }

    private static Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Sunucu Hatası",
            Detail = "Beklenmedik bir hata oluştu. Lütfen daha sonra tekrar deneyin."
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
