using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Models
{
    public class Fuel
    {
        [JsonPropertyName("gas(euro/MWh)")]
        public double Gasfired { get; set; }

        [JsonPropertyName("kerosine(euro/MWh)")]
        public double Turbojet { get; set; }

        [JsonPropertyName("co2(euro/ton)")]
        public int Co2 { get; set; }

        [JsonPropertyName("wind(%)")]
        public int Windturbine { get; set; }
    }
}