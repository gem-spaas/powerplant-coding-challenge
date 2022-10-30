using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Models
{
    public class PayloadResponse
    {
        public string Name { get; set; }
        public double P { get; set; }

        [JsonIgnore]
        public double UnitCost { get; set; }

        [JsonIgnore]
        public bool Activable { get; set; }
    }
}
