using Domain.Entities;
using Domain.Value_Objects;

namespace Domain.Services
{
    public static class MeritOrder
    {
        public static IReadOnlyList<PowerPlant> Sort(
            IEnumerable<PowerPlant> powerPlants,
            Fuel fuels)
        {
            return [.. powerPlants.OrderBy(plant => plant.CalculateMerit(fuels))];
        }
    }
}
