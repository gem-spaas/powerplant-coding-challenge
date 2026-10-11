using System.Text.Json.Serialization;

namespace Application.Use_Cases.ProductionPlan
{
    public record class FuelDto(
    [property: JsonPropertyName("gas(euro/MWh)")]
    double Gas,

    [property: JsonPropertyName("kerosine(euro/MWh)")]
    double Kerosine,

    [property: JsonPropertyName("co2(euro/ton)")]
    double Co2,

    [property: JsonPropertyName("wind(%)")]
    double Wind);
}
