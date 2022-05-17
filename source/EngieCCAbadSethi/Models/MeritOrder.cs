namespace CalculatePowerGenerationByPowerPlants.Models
{
    public class MeritOrder
    {
        public string PlantName { get; set; }
        public string PlantType { get; set; }
        public double CostPerMWh { get; set; }
        public double CostPmin { get; set; }
        public double CostPmax { get; set; }
        public double Pmin { get; set; }
        public double Pmax { get; set; } 
    }
}
