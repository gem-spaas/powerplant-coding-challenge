using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PowerPlantCodingChallenge.ProductionPlan
{
    public class Fuels
    {
        [JsonProperty("gas(euro/MWh)")]
        [Required]
        [Range(0, float.PositiveInfinity)]
        public float? GasCostPerMWh { get; set; }

        [JsonProperty("kerosine(euro/MWh)")]
        [Required]
        [Range(0, float.PositiveInfinity)]
        public float? KerosineCostPerMWh { get; set; }

        [JsonProperty("co2(euro/ton)")]
        [Required]
        [Range(0, float.PositiveInfinity)]
        public float? Co2EmissionCostPerTon { get; set; }

        [JsonProperty("wind(%)")]
        [Required]
        [Range(0, float.PositiveInfinity)]
        public float? WindPercentage { get; set; }
    }
}
