using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Domain.Request
{
    public sealed class ProductionPlanRequest
    {
        [JsonPropertyName("load")]
        public decimal Load { get; init; }

        [JsonPropertyName("fuels")]
        public FuelPrices Fuels { get; init; } = new();

        [JsonPropertyName("powerplants")]
        public List<PowerPlant> PowerPlants { get; init; } = [];
    }
}
