using MediatR;
using PowerPlantCC.Domain.Models;

namespace PowerPlantCC.Application.ProductionPlan.Queries.GetProductionPlan;

public class GetProductionPlanQuery : IRequest<GetProductionPlanResponse[]>
{
    public int Load { get; set; }
    public Fuels Fuels { get; set; } = null!;
    public PowerPlant[] PowerPlants { get; set; } = null!;
}