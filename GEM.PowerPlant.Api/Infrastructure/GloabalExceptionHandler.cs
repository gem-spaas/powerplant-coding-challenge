using GEM.PowerPlant.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Infrastructure
{
	public class GloabalExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<GloabalExceptionHandler> _logger;

		public GloabalExceptionHandler(ILogger<GloabalExceptionHandler> logger)
		{
			this._logger = logger;
		}
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			_logger.LogError(exception, exception.Message);

			ProblemDetails problemDetails;

			if (exception is DomainException domainException)
			{
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status400BadRequest,
					Type = "https://www.rfc-editor.org/rfc/rfc7231.html#section-6.5.1",
					Title = domainException.Message ?? "Someting went wrong",
					Detail = domainException.Message
				};
				httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
			}
			else
			{
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status500InternalServerError,
					Type = "https://www.rfc-editor.org/rfc/rfc7231.html#section-6.6.1",
					Title = "Server Error"
				};

				httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
			}



			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

			return true;
		}
	}
}
