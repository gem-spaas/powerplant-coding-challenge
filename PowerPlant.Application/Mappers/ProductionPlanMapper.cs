using Mapster;
using PowerPlant.Application.Queries.ProductionPlan;
using PowerPlant.Domain.Entities;

namespace PowerPlant.Application.Mappers;

public class ProductionPlanMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UnitCommitment, ProductionPlanResponse>()
            .Map(
                dest => dest.P, 
                src => src.Power);
    }
}