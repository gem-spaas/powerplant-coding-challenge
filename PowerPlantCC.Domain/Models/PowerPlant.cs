using PowerPlantCC.Domain.Enums;

namespace PowerPlantCC.Domain.Models;

public class PowerPlant
{
    public string Name { get; set; } = null!;
    public PowerPlantType Type { get; set; }
    public decimal Efficiency { get; set; }
    public decimal Pmin { get; set; }
    public decimal Pmax { get; set; }
    public decimal Power { get; set; } = 0;
    public decimal Cost { get; set; } = 0;

    public void InitCostAndBoundaries(Fuels fuels)
    {
        Cost = GetPowerPlantCost(fuels);
        Pmin = Math.Ceiling(Pmin * 10) / 10;

        if (Type == PowerPlantType.windturbine)
            Pmax *= fuels.Wind / 100.0m;

        Pmax = Math.Floor(Pmax * 10) / 10;
    }

    public void UseDeliverablePower(decimal powerRequired)
    {
        Power = Math.Max(Math.Min(powerRequired, Pmax), Pmin);
    }

    public void ReducePossiblePower(decimal powerToRemove)
    {
        decimal removablePower = Power - Pmin;
        decimal removedPower = powerToRemove > removablePower ? removablePower : powerToRemove;

        Power -= removedPower;
    }

    public decimal GetPowerPlantCost(Fuels fuels)
    {
        if (Type == PowerPlantType.windturbine)
            return 0;

        decimal fuelCost = Type == PowerPlantType.gasfired ? fuels.Gas : fuels.Kerosine;
        decimal co2Cost = Type == PowerPlantType.gasfired ? 0.3M * fuels.Co2 : 0M;

        return (fuelCost / Efficiency) + co2Cost;
    }
}