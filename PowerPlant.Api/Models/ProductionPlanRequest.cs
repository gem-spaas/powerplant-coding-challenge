using System.Text.Json.Serialization;

namespace PowerPlant.Api.Models;

public record ProductionPlanRequest
{
    [JsonPropertyName("load")]
    public decimal Load { get; set; }
    
    [JsonPropertyName("fuels")]
    public FuelsDTO Fuels { get; set; } = null!;

    [JsonPropertyName("powerplants")]
    public List<PowerPlantDTO> PowerPlants { get; set; } = null!;
}