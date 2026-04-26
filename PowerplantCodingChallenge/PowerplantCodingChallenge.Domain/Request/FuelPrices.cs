using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Domain.Request
{
    public sealed class FuelPrices
    {
        [JsonPropertyName("gas(euro/MWh)")]
        public decimal GasEuroPerMWh { get; init; }

        [JsonPropertyName("kerosine(euro/MWh)")]
        public decimal KerosineEuroPerMWh { get; init; }

        [JsonPropertyName("co2(euro/ton)")]
        public decimal Co2EuroPerTon { get; init; }

        [JsonPropertyName("wind(%)")]
        public decimal WindPercentage { get; init; }
    }
}
