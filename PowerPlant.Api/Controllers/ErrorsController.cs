using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PowerPlant.Application.Exceptions;

namespace PowerPlant.Api.Controllers;

[ApiController]
[Route("/error")]
public class ErrorsController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Problem(
            title: exception?.Message, 
            statusCode: exception switch
            {
              ValidationException => 400,
              BusinessException => 400,
              _ => 500
            }
        );
    }
}