using Newtonsoft.Json;

namespace PowerPlantCodingChallenge.ProductionPlan
{
    public class UnitCommitment
    {
        public string Name { get; set; }

        [JsonProperty("p")]
        public float Power { get; set; }
    }
}
