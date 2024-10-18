using BusinessLogic.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.Models
{
    public class PowerPlantInput
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public TypePowerEnum Type { get; set; }

        [Required]
        public decimal Efficiency { get; set; }

        [Required]
        public int Pmin { get; set; }

        [Required]
        public int Pmax { get; set; }
    }
}
