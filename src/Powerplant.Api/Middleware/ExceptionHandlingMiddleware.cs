using Microsoft.AspNetCore.Mvc;

namespace Powerplant.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            _logger.LogWarning("Request was cancelled by the client. TraceId: {TraceId}", context.TraceIdentifier);
        }
        catch (BadHttpRequestException exception)
        {
            _logger.LogWarning(exception, "Bad request. TraceId: {TraceId}", context.TraceIdentifier);
            await WriteProblemDetailsAsync(context, StatusCodes.Status400BadRequest, "Bad request", exception);
        }
        catch (NotImplementedException exception)
        {
            _logger.LogInformation(exception, "Feature not implemented. TraceId: {TraceId}", context.TraceIdentifier);
            await WriteProblemDetailsAsync(context, StatusCodes.Status501NotImplemented, "Not implemented", exception);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled runtime error. TraceId: {TraceId}", context.TraceIdentifier);
            await WriteProblemDetailsAsync(context, StatusCodes.Status500InternalServerError, "Unexpected server error", exception);
        }
    }

    private async Task WriteProblemDetailsAsync(
        HttpContext context,
        int statusCode,
        string title,
        Exception exception)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogWarning("The response has already started; problem details cannot be written.");
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = _environment.IsDevelopment() ? exception.Message : null,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}
