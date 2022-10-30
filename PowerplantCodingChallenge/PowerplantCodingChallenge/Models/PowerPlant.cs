using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Models
{
    public class PowerPlant
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Type")]
        public PowerPlantType Type { get; set; }
        public double Efficiency { get; set; }
        [JsonPropertyName("PMin")]
        public int PowerMin { get; set; }
        [JsonPropertyName("PMax")]
        public int PowerMax { get; set; }
    }
}