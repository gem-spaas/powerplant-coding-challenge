using PowerPlant.Application.PowerPlants;
using PowerPlant.Domain;
using PowerPlant.Domain.Planning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Application.ProductionPlanning;

internal static class PlanningMapper
{
    public static Fuels ToDomain(this FuelsDto fuels) => new(fuels.Gas, fuels.Kerosine, fuels.Co2, fuels.WindPercentage);

    public static IReadOnlyList<Domain.PowerPlants.PowerPlant> ToDomain(this IReadOnlyList<PowerPlantDto> powerplants,
        IPowerPlantFactory factory)
    {
        return powerplants.Select(plant => factory.Create(plant.Type, plant.Name, plant.Efficiency, plant.Pmin, plant.Pmax)).ToList();
    }

    public static IReadOnlyList<ProductionPlanResponseItem> ToResponse(this IReadOnlyList<PowerOutput> outputs)
    {
        return outputs.Select(output => new ProductionPlanResponseItem(output.Name, output.Power)).ToList();
    }
}
