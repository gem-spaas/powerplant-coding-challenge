using System.ComponentModel.DataAnnotations;

namespace PowerplantCodingChallenge.Models
{
    public class Payload
    {
        [Required]
        [Range(0, Double.PositiveInfinity)]
        public int Load { get; set; }
        [Required]
        public Fuel Fuels { get; set; }
        [Required]
        public List<PowerPlant> PowerPlants { get; set; }

    }
}
