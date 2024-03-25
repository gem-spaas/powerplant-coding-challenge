using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PowerPlantCC.Application.Common.Exceptions;

namespace PowerPlantCC.Api.Controllers;

[ApiController()]
[Route("/error")]
public class ErrorsController : ControllerBase
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        string title = exception?.Message ?? "An error has occured.";

        var statusCode = exception switch
        {
            ValidationException => 400,
            BusinessException => 400,
            _ => 500
        };

        return Problem(statusCode: statusCode, title: title);
    }
}