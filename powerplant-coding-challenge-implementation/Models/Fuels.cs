using System.Text.Json.Serialization;

namespace powerplant_coding_challenge_implementation.Models
{
    public class Fuels
    {
        [JsonPropertyName("gas(euro/MWh)")]
        public float Gas { get; set; }
        [JsonPropertyName("kerosine(euro/MWh)")]
        public float Kerozine { get; set; }
        [JsonPropertyName("co2(euro/ton)")]
        public float CO2 { get; set; }
        [JsonPropertyName("wind(%)")]
        public int Wind { get; set; }
    }
}