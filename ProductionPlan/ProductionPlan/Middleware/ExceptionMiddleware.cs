using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ProductionPlan.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate nextDelegate)
    {
        _next = nextDelegate;
    }

    public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {
        
        try
        {
           await _next(context);
        }
        catch (ArgumentNullException e)
        {
            await WriteResponseAsync(context, e.Message, HttpStatusCode.BadRequest);
        }
        catch (InvalidOperationException e)
        {
            await WriteResponseAsync(context, e.Message, HttpStatusCode.UnprocessableEntity);
        }
        catch (Exception e)
        {
            await WriteResponseAsync(context, e.Message, HttpStatusCode.InternalServerError);
        }
    }

    private async Task WriteResponseAsync(HttpContext context, string message, HttpStatusCode statusCode)
    {
        context.Response.StatusCode = (int)statusCode;

        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(message));
    }
}