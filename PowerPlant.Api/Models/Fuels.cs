using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace powerplant.Models
{
    public class Fuels
    {
        [JsonPropertyName("gas(euro/MWh)")]
        public double GasPrice { get; set; }

        [JsonPropertyName("kerosine(euro/MWh)")]
        public double KerosinePrice { get; set; }

        [JsonPropertyName("co2(euro/ton)")]
        public double Co2euroton { get; set; }

        [JsonPropertyName("wind(%)")]
        public double Wind { get; set; }
    }
}
