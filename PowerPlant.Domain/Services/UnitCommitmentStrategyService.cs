using PowerPlant.Domain.Entities;
using PowerPlant.Domain.Interfaces;

namespace PowerPlant.Domain.Services;

public class UnitCommitmentStrategyService : IUnitCommitmentStrategyService
{
    public List<UnitCommitment> Resolve(ProductionPlan productionPlan)
    {
        var sortedPowerPlants = SortByMeritOrder(productionPlan.PowerPlants).ToArray();
        var unitCommitments = new List<UnitCommitment>();
        var remainingLoad = productionPlan.Load;

        for (var i = 0; i < sortedPowerPlants.Length; i++)
        {
            var powerPlant = sortedPowerPlants[i];

            if (remainingLoad > 0)
            {
                remainingLoad = ManageRemainingLoad(powerPlant, remainingLoad, sortedPowerPlants, i, ref unitCommitments);
            }
            else
            {
                unitCommitments.Add(new UnitCommitment(powerPlant.Name, 0));
            }
        }

        return unitCommitments;
    }

    private decimal ManageRemainingLoad(
        Entities.PowerPlant powerPlant, 
        decimal remainingLoad, 
        Entities.PowerPlant[] sortedPowerPlants, 
        int index,
        ref List<UnitCommitment> unitCommitments)
    {
        var power = Math.Max(powerPlant.Pmin, Math.Min(powerPlant.Pmax, remainingLoad));

        if (power == powerPlant.Pmax)
        {
            unitCommitments.Add(new UnitCommitment(powerPlant.Name, powerPlant.Pmax));
            remainingLoad -= powerPlant.Pmax;
        }
        else if (power == remainingLoad)
        {
            unitCommitments.Add(new UnitCommitment(powerPlant.Name, remainingLoad));
            remainingLoad = 0;
        }
        else
        {
            var exceedingAmountOfPower = powerPlant.Pmin - remainingLoad;
            var isBalancingSolvable =
                TryPowerBalancing(sortedPowerPlants, ref unitCommitments, exceedingAmountOfPower, index - 1);

            if (isBalancingSolvable)
            {
                unitCommitments.Add(new UnitCommitment(powerPlant.Name, powerPlant.Pmin));
                remainingLoad = 0;
            }
        }

        return remainingLoad;
    }

    private static IEnumerable<Entities.PowerPlant> SortByMeritOrder(IEnumerable<Entities.PowerPlant> powerPlants)
    {
        return powerPlants.OrderBy(p => p.Cost);
    }

    private bool TryPowerBalancing(
        Entities.PowerPlant[] sortedPowerPlants,
        ref List<UnitCommitment> unitCommitments, 
        decimal exceedingAmountOfPower, 
        int index)
    {
        while (index >= 0 && exceedingAmountOfPower > 0)
        {
            var powerPlant = sortedPowerPlants[index];
            var matchingUnitCommitment = unitCommitments[index];
            if (matchingUnitCommitment.Power > powerPlant.Pmin)
            {
                var powerAvailableToBeRemoved = matchingUnitCommitment.Power - powerPlant.Pmin;
                var powerToBalance = Math.Min(powerAvailableToBeRemoved, exceedingAmountOfPower);
                matchingUnitCommitment.AdjustPower(matchingUnitCommitment.Power - powerToBalance);
                exceedingAmountOfPower -= powerToBalance;
            }
            index--;
        }
        return exceedingAmountOfPower == 0;
    }
}