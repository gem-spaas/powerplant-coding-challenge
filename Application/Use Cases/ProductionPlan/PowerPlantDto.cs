namespace Application.Use_Cases.ProductionPlan
{
    public record class PowerPlantDto(string Name, string Type, double Efficiency, double Pmin, double Pmax);
}
