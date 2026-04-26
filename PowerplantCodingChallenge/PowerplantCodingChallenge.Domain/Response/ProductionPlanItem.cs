using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Domain.Response
{
    public sealed class ProductionPlanItem
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("p")]
        public decimal Power { get; init; }
    }
}
