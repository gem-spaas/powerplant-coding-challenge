using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PowerPlantCodingChallenge.ProductionPlan
{
    public class PowerLoadForecast
    {
        [Required]
        [Range(0, float.PositiveInfinity)]
        public float? Load { get; set; }

        [Required]
        public Fuels Fuels { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "The PowerPlants field must have at least one item.")]
        public IEnumerable<PowerPlant> PowerPlants;
    }
}
