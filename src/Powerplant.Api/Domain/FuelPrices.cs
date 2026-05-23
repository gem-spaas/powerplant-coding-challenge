namespace Powerplant.Api.Domain
{
    public class FuelPrices
    {
        public required decimal GasEuroPerMWh { get; init; }

        public required decimal KerosineEuroPerMWh { get; init; }

        public required decimal Co2EuroPerTon { get; init; }

        public required decimal WindPercentage { get; init; }
    }
}
