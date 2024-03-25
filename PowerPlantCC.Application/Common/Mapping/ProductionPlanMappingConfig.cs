using Mapster;
using PowerPlantCC.Application.ProductionPlan.Queries.GetProductionPlan;
using PowerPlantCC.Domain.Models;

namespace PowerPlantCC.Application.Common.Mapping;

public class ProductionPlanMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PowerPlant, GetProductionPlanResponse>()
            .Map(dest => dest.P, src => src.Power);
    }
}