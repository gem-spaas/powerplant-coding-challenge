using Application.Use_Cases.ProductionPlan.Mappings;
using Domain.Services;
using MediatR;
using SharedKernel;

namespace Application.Use_Cases.ProductionPlan
{
    internal sealed class ProductionPlanHandler(ProductionPlanCalculator productionPlanCalculator) : IRequestHandler<CreateProductionPlanCommand, Result<IReadOnlyCollection<ResultProductionPlanResponse>>>
    {
        public async Task<Result<IReadOnlyCollection<ResultProductionPlanResponse>>> Handle(
            CreateProductionPlanCommand request, 
            CancellationToken cancellationToken)
        {
            var powerplants = request.Powerplants.Select(p => p.ToDomain()).ToList();
            var fuels = request.Fuels.ToDomain();

            var result = productionPlanCalculator.Calculate(request.Load, powerplants, fuels);

            if (result.IsFailure)
            {
                return Result<IReadOnlyCollection<ResultProductionPlanResponse>>.Failure(result.Error);
            }

            var plantProductionResponse = result!.Value!
                .Select(plant => new ResultProductionPlanResponse(
                    plant.Name,
                    plant.Power))
                .ToList();

            return Result<IReadOnlyCollection<ResultProductionPlanResponse>>.Success(plantProductionResponse);
        }
    }
}
