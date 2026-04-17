using System.Net;
using System.Text.Json;
using FinanceTracker.Domain.Exceptions;
using FluentValidation;

namespace FinanceTracker.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation failed: {Errors}", ex.Errors);
            await WriteResponse(context, HttpStatusCode.UnprocessableEntity, new
            {
                type = "validation_error",
                errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
            });
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain rule violated: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.BadRequest, new
            {
                type = "domain_error",
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteResponse(context, HttpStatusCode.InternalServerError, new
            {
                type = "server_error",
                message = "An unexpected error occurred."
            });
        }
    }

    private static Task WriteResponse(HttpContext context, HttpStatusCode statusCode, object body)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(JsonSerializer.Serialize(body, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
