namespace Powerplant.Api.Domain
{
    public class PowerPlant
    {
        public required string Name { get; init; }

        public required PowerPlantType Type { get; init; }

        public required decimal Efficiency { get; init; }

        public required decimal PMin { get; init; }

        public required decimal PMax { get; init; }

        public decimal CostPerMWh(FuelPrices fuelPrices)
        {
            return Type switch
            {
                PowerPlantType.GasFired => (fuelPrices.GasEuroPerMWh / Efficiency) + (fuelPrices.Co2EuroPerTon * 0.3m),
                PowerPlantType.TurboJet => (fuelPrices.KerosineEuroPerMWh / Efficiency) + (fuelPrices.Co2EuroPerTon * 0.3m),
                PowerPlantType.WindTurbine => 0,
                _ => throw new InvalidOperationException($"Unknown power plant type: {Type}")
            };
        }

        public decimal GetPossibleLoad(decimal load, FuelPrices fuelPrices)
        {
            if (load < PMin)
            {
                // If the load is less than the minimum power output, we cannot produce any power
                return 0;
            }

            if (Type == PowerPlantType.WindTurbine)
            {
                // Wind turbines can produce up to their maximum capacity regardless of fuel prices
                return Math.Min(PMax*fuelPrices.WindPercentage/100, load);
            }
            // For other power plants, we can produce up to PMax or the remaining load, whichever is smaller
            return Math.Min(PMax, load);
        }
    }
}
