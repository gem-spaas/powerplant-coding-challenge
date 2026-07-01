using System.Text.Json.Serialization;

namespace power_plant_coding_challenge_domain.Models;

public class Fuel
{
    [JsonPropertyName("gas(euro/MWh)")]
    public decimal Gas { get; set; }

    [JsonPropertyName("kerosine(euro/MWh)")]
    public decimal Kerosine { get; set; }

    [JsonPropertyName("co2(euro/ton)")]
    public decimal Co2 { get; set; }

    [JsonPropertyName("wind(%)")]
    public decimal Wind { get; set; }
}
