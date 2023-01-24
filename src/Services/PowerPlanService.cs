using PowerplantChallenge.Model;

namespace PowerplantChallenge.Services {
    public class PowerPlanService
    {
        public IEnumerable<Powerplant> GetPowerPlan(IEnumerable<Powerplant> powerplants, int neededLoad){
            powerplants = powerplants.OrderBy(
                pp => pp.BaseCost)
            .ThenBy(pp => pp.MinPower)
            .ThenBy(pp => pp.MaxPower);

            var finalList = new List<Powerplant>();

            List<Powerplant> reservedPowerplant = new List<Powerplant>();

            foreach (var powerplant in powerplants)
            {
                if(finalList.Sum(fl => fl.ProposedLoad) >= neededLoad){
                    powerplant.ProposedLoad = 0;
                    finalList.Add(powerplant);
                }
                else if(powerplant.MinPower > 0 && finalList.Sum(fl => fl.ProposedLoad) + powerplant.MinPower > neededLoad){
                    reservedPowerplant.Add(powerplant);
                }
                else{
                    var loadToFill = neededLoad - finalList.Sum(fl => fl.ProposedLoad);
                    var proposedLoad = loadToFill >= powerplant.MaxPower ? powerplant.MaxPower : loadToFill;

                    var costAtProposedLoad = powerplant.BaseCost * proposedLoad;
                    var alternative = reservedPowerplant.FirstOrDefault(rpp => rpp.BaseCost * rpp.MinPower <= costAtProposedLoad);
                    if(alternative != null){
                        alternative.ProposedLoad = alternative.MinPower;
                        finalList.Add(alternative);
                        reservedPowerplant.Remove(alternative);
                    }
                    else {
                        powerplant.ProposedLoad = proposedLoad;
                        finalList.Add(powerplant);
                    }
                }
            }

            foreach (var powerplant in reservedPowerplant){
                powerplant.ProposedLoad = 0;
                finalList.Add(powerplant);
            }

            return finalList;
        }
    }
}