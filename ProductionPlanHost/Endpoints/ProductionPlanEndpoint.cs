using Application.Use_Cases.ProductionPlan;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProductionPlanHost.Endpoints
{
    public sealed class ProductionPlanEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/production-plan",               
                async (CreateProductionPlanCommand command,[FromServices] ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                return Results.BadRequest(result);                  
            })
                .Produces<IReadOnlyCollection<ResultProductionPlanResponse>>()
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
