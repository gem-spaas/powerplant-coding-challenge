using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PowerPlantCodingChallenge.ProductionPlan
{
    public class PowerPlant
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [JsonProperty("type")]
        [Required]
        public PowerPlantPowerType PowerType { get; set; }

        [Required]
        [Range(0, float.PositiveInfinity)]
        public float? Efficiency { get; set; }

        [Required]
        [Range(0, float.PositiveInfinity)]
        public float? Pmin { get; set; }

        [Required]
        [Range(0, float.PositiveInfinity)]
        public float? Pmax { get; set; }
    }
}
