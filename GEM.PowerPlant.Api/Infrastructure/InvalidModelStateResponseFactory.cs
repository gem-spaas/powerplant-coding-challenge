using Microsoft.AspNetCore.Mvc;

namespace WebApi.Infrastructure
{
	public static class InvalidModelStateResponseFactory
	{
		public static IActionResult Build(ActionContext actionContext) => new BadRequestObjectResult(TypedResults.ValidationProblem(actionContext.ModelState
				.ToDictionary(ms => ms.Key, ms => ms.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? [])));

	}
}
