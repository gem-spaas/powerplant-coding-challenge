using System.Text.Json.Serialization;

namespace powerplant_coding_challenge_implementation.Models
{
    public class ProductionPlanResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("p")]
        public int Production { get; set; }
    }
}
