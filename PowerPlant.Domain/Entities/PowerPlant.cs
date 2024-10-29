using PowerPlant.Domain.Enums;

namespace PowerPlant.Domain.Entities;

public class PowerPlant
{
    public string Name { get; set; } = null!;
    
    public PowerPlantType Type { get; set; }
    
    public decimal Efficiency { get; set; }

    public decimal Pmin { get; set; }

    public decimal Pmax { get; set; }

    public decimal Power { get; set; }

    public decimal Cost { get; set; }
}