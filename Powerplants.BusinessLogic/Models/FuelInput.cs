using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusinessLogic.Models
{
    public class FuelInput
    {
        [Required]
        [JsonPropertyName("gas(euro/MWh)")]
        public decimal GasEuroPerMWh { get; set; }

        [Required]
        [JsonPropertyName("kerosine(euro/MWh)")]
        public decimal KerosineEuroPerMWh { get; set; }

        [Required]
        [JsonPropertyName("co2(euro/ton)")]
        public decimal Co2EuroPerTon { get; set; }

        [Required]
        [JsonPropertyName("wind(%)")]
        public decimal WindPercentage { get; set; }
    }
}
