using PowerPlant.Application.PowerPlants;
using PowerPlant.Domain.Planning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Application.ProductionPlanning;

public sealed class ProductionPlanService(IPowerPlantFactory powerPlantFactory, IProductionPlanner productionPlanner) : IProductionPlanService
{
    public IReadOnlyList<ProductionPlanResponseItem> CalculateProductionPlan(ProductionPlanRequest request)
    {
        var fuels = request.Fuels.ToDomain();
        var plants = request.Powerplants.ToDomain(powerPlantFactory);

        var outputs = productionPlanner.Plan(request.Load, plants, fuels);

        return outputs.ToResponse();
    }
}
