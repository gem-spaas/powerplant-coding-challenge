using PowerplantCodingChallenge.Domain.Request.Enumeration;
using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Domain.Request
{
    public sealed class PowerPlant
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PlantType Type { get; init; }

        [JsonPropertyName("efficiency")]
        public decimal Efficiency { get; init; }

        [JsonPropertyName("pmin")]
        public decimal PowerMin { get; init; }

        [JsonPropertyName("pmax")]
        public decimal PowerMax { get; init; }
    }
}
