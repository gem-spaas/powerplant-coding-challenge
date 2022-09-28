using System.Text.Json.Serialization;

namespace EngieApi;

public class ProductionPlanResponse
{
    public List<ProductionPlan> ProductionPlans { get; set; }
}

public class ProductionPlan {
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("p")] public int P { get; set; }
}
