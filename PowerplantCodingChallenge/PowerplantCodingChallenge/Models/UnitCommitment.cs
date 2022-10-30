using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Models
{
    public class UnitCommitment
    {
        public UnitCommitment(string name, double unitCost, bool activable)
        {
            Name = name;
            UnitCost = unitCost;
            Activable = activable;
        }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("P")]
        public double Power { get; set; }

        [JsonIgnore]
        public double UnitCost { get; set; }

        [JsonIgnore]
        public bool Activable { get; set; }
    }
}
