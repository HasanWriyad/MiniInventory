using FluentValidation;
using MiniInventory.Application.Common.Models;

namespace MiniInventory.API.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex, logger);
        }
    }

    private static async Task HandleAsync(
        HttpContext ctx, Exception ex, ILogger logger)
    {
        var (status, message, errors) = ex switch
        {
            ValidationException ve => (400, "Validation failed",
                (IEnumerable<string>?)ve.Errors.Select(e => e.ErrorMessage)),
            UnauthorizedAccessException => (401, ex.Message, (IEnumerable<string>?)null),
            KeyNotFoundException        => (404, ex.Message, (IEnumerable<string>?)null),
            InvalidOperationException   => (400, ex.Message, (IEnumerable<string>?)null),
            _                           => (500, "An unexpected error occurred.", (IEnumerable<string>?)null)
        };

        if (status == 500)
            logger.LogError(ex, "Unhandled exception");

        ctx.Response.StatusCode  = status;
        ctx.Response.ContentType = "application/json";

        await ctx.Response.WriteAsJsonAsync(new ErrorResponse
        {
            Status  = status,
            Message = message,
            Errors  = errors
        });
    }
}
