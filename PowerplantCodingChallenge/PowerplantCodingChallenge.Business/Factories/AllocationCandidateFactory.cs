using PowerplantCodingChallenge.Business.Extensions;
using PowerplantCodingChallenge.Business.Models;
using PowerplantCodingChallenge.Domain.Request;
using PowerplantCodingChallenge.Domain.Request.Enumeration;

namespace PowerplantCodingChallenge.Business.Factories
{
    internal static class AllocationCandidateFactory
    {
        public static List<AllocationCandidate> CreateCandidates(ProductionPlanRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (request.Fuels == null)
            {
                throw new ArgumentNullException(nameof(request.Fuels));
            }
            if (request.PowerPlants == null)
            {
                throw new ArgumentNullException(nameof(request.PowerPlants));
            }

            return request.PowerPlants.Select(pp => CreateCandidate(pp, request.Fuels)).ToList();
        }

        private static AllocationCandidate CreateCandidate(PowerPlant powerPlant, FuelPrices fuels)
            => new AllocationCandidate
            {
                Name = powerPlant.Name,
                Type = powerPlant.Type,
                MarginalCost = GetMarginalCost(powerPlant, fuels),
                PowerMinTenths = powerPlant.PowerMin.ToTenths(),
                PowerMaxTenths = GetAvailablePowerMax(powerPlant, fuels).ToTenths(),
                AllocatedPowerTenths = 0
            };

        private static decimal GetAvailablePowerMax(PowerPlant powerPlant, FuelPrices fuels)
            => powerPlant.Type switch
            {
                PlantType.WindTurbine => powerPlant.PowerMax * fuels.WindPercentage / 100m,
                _ => powerPlant.PowerMax
            };

        private static decimal GetMarginalCost(PowerPlant powerPlant, FuelPrices fuels)
            => powerPlant.Type switch
            {
                PlantType.WindTurbine => 0m,
                PlantType.GasFired => fuels.GasEuroPerMWh / powerPlant.Efficiency,
                PlantType.TurboJet => fuels.KerosineEuroPerMWh / powerPlant.Efficiency,
                _ => throw new InvalidOperationException($"Unknown plant type : {powerPlant.Type}")
            };
    }
}
