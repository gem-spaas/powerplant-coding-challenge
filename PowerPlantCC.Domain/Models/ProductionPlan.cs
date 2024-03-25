namespace PowerPlantCC.Domain.Models;

public class ProductionPlan
{
    public decimal Load { get; set; }
    public Fuels Fuels { get; set; } = null!;
    public PowerPlant[] PowerPlants { get; set; } = null!;
    public decimal CurrentPower => PowerPlants.Sum(p => p.Power);

    public void Build()
    {
        for (int i = 0; i < PowerPlants.Length; i++)
        {
            PowerPlants[i].InitCostAndBoundaries(Fuels);
        }

        PowerPlants = PowerPlants
            .OrderBy(p => p.Cost)
            .ToArray();

        for (int i = 0; i < PowerPlants.Length; i++)
        {
            PowerPlant powerPlant = PowerPlants[i];

            if (CurrentPower < Load)
            {
                decimal powerNeededLeft = Load - CurrentPower;
                powerPlant.UseDeliverablePower(powerNeededLeft);

                if (CurrentPower > Load)
                {
                    bool balanceSucceed = BalanceUsedPower(i - 1);

                    if (!balanceSucceed)
                        break;
                }
            }
        }
    }

    private bool BalanceUsedPower(int iSearch)
    {
        while (iSearch >= 0 && CurrentPower > Load)
        {
            decimal powerToBalance = CurrentPower - Load;
            PowerPlant powerPlant = PowerPlants[iSearch];

            powerPlant.ReducePossiblePower(powerToBalance);

            iSearch--;
        }

        return CurrentPower <= Load;
    }
}