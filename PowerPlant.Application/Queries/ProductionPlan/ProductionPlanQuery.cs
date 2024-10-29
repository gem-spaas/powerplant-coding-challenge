using MediatR;
using PowerPlant.Domain.Entities;

namespace PowerPlant.Application.Queries.ProductionPlan;

public class ProductionPlanQuery : IRequest<List<ProductionPlanResponse>>
{
    public decimal Load { get; set; }
    public Fuels Fuels { get; set; } = null!;
    public List<Domain.Entities.PowerPlant> PowerPlants { get; set; } = new();
}