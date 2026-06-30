using System.Text.Json;
using PowerPlant.Domain.Exceptions;

namespace PowerPlant.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception) when (exception is InfeasibleLoadException or ArgumentException)
        {
            logger.LogWarning(exception, "Rejected an invalid production plan request.");
            await WriteProblem(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An unexpected error occurred while computing the production plan.");
            await WriteProblem(context, StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblem(HttpContext context, int statusCode, string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = JsonSerializer.Serialize(new { status = statusCode, detail });
        await context.Response.WriteAsync(payload);
    }
}