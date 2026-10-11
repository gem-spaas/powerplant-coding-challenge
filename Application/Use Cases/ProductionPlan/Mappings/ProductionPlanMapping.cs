using Domain.Entities;
using Domain.Value_Objects;

namespace Application.Use_Cases.ProductionPlan.Mappings
{
    public static class ProductionPlanMapping
    {
        public static PowerPlant ToDomain(
        this PowerPlantDto dto)
        {
            return PowerPlant.Create(dto.Name, dto.Efficiency, dto.Type, dto.Pmin, dto.Pmax);
        }

        public static Fuel ToDomain(this FuelDto dto) {
            return Fuel.Create(dto.Gas, dto.Kerosine, dto.Co2, dto.Wind);
        }
    }
}
