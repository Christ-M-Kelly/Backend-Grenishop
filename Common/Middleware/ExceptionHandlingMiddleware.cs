using System.Net;
using System.Text.Json;
using BackendGrenishop.Common.Exceptions;

namespace BackendGrenishop.Common.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new ErrorResponse();

        switch (exception)
        {
            case NotFoundException notFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Message = notFoundException.Message;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                _logger.LogWarning(notFoundException, "Resource not found: {Message}", notFoundException.Message);
                break;

            case BadRequestException badRequestException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = badRequestException.Message;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                _logger.LogWarning(badRequestException, "Bad request: {Message}", badRequestException.Message);
                break;

            case UnauthorizedException unauthorizedException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Message = unauthorizedException.Message;
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                _logger.LogWarning(unauthorizedException, "Unauthorized access: {Message}", unauthorizedException.Message);
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Message = "Une erreur interne est survenue. Veuillez réessayer plus tard.";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
}
