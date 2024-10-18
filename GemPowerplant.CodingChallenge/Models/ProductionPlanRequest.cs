using System.ComponentModel.DataAnnotations;

namespace GemPowerplant.CodingChallenge.Models
{
    public class ProductionPlanRequest
    {
        [Required]
        [Range(1, double.MaxValue, ErrorMessage ="Load must be higher than 0")]
        public double Load { get; set; }

        [Required]
        public Fuels? Fuels { get; set; }

        [Required]
        public List<Powerplant>? Powerplants { get; set; }
    }
}
