using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class ProductPlanInput
    {
        [Required]
        public int Load { get; set; }

        [Required]
        public FuelInput Fuels { get; set; }

        [Required]
        public List<PowerPlantInput> Powerplants { get; set; }
    }
}
