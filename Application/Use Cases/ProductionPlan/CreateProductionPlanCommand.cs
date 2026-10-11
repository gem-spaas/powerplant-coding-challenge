using MediatR;
using SharedKernel;
namespace Application.Use_Cases.ProductionPlan
{
    public record class CreateProductionPlanCommand(
         double Load,
         IReadOnlyCollection<PowerPlantDto> Powerplants,
         FuelDto Fuels)
        : IRequest<Result<IReadOnlyCollection<ResultProductionPlanResponse>>>;
}
