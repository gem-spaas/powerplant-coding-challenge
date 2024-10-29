using System.Text.Json.Serialization;
using PowerPlant.Api.Enums;

namespace PowerPlant.Api.Models;

public record PowerPlantDTO
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter<PowerPlantTypeDTO>))]
    public PowerPlantTypeDTO Type { get; set; }
    
    [JsonPropertyName("efficiency")]
    public decimal Efficiency { get; set; }
    
    [JsonPropertyName("pmin")]
    public decimal Pmin { get; set; }
    
    [JsonPropertyName("pmax")]
    public decimal Pmax { get; set; }
}