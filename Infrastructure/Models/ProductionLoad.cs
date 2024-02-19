using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace powerplant_coding_challenge.Infrastructure.Models
{
    public class ProductionLoad
    {
        public int Load { get; set; }
        public Fuels Fuels { get; set; } = null!;
        public List<Powerplant> PowerPlants { get; set; } = null!;
    }

    public class Fuels
    {
        [JsonPropertyName("gas(euro/MWh)")]
        public float GasEuroMWh { get; set; }
        [JsonPropertyName("kerosine(euro/MWh)")]
        public float KerosineEuroMWh { get; set; }
        [JsonPropertyName("co2(euro/ton)")]
        public int CO2EuroTon { get; set; }
        [JsonPropertyName("wind(%)")]
        public int Wind { get; set; }
    }

    public class Powerplant
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public float Efficiency { get; set; }
        public int PMin { get; set; }
        public int PMax { get; set; }
    }
}
