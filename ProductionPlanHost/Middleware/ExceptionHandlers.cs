using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProductionPlanHost.Middleware
{
    public sealed class ExceptionHandlers : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails problemDetails;

            switch (exception)
            {
                case Application.Exceptions.ValidationExceptions validationException:
                    {
                        var errors = validationException.Errors
                            .GroupBy(error => error.PropertyName)
                            .ToDictionary(
                                group => group.Key,
                                group => group
                                    .Select(error => error.ErrorMessage)
                                    .Distinct()
                                    .ToArray());

                        problemDetails = new ValidationProblemDetails(errors)
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "One or more validation errors occurred.",
                            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                            Instance = httpContext.Request.Path
                        };

                        break;
                    }

                case DomainException domainException:
                    {
                        problemDetails = new ProblemDetails
                        {
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Title = "Business rule violation",
                            Detail = domainException.Message,
                            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.21",
                            Instance = httpContext.Request.Path
                        };

                        break;
                    }

                default:
                    {
                        problemDetails = new ProblemDetails
                        {
                            Status = StatusCodes.Status500InternalServerError,
                            Title = "An unexpected error occurred.",
                            Detail = "An internal server error occurred.",
                            Instance = httpContext.Request.Path
                        };

                        break;
                    }
            }

            httpContext.Response.StatusCode =
                problemDetails.Status!.Value;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

            return true;
        }
    }
}
