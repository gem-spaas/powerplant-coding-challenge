using System.Text.Json.Serialization;

public class ProductionPlanResponseElement
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("p")] public decimal P { get; set; }
}