using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace GemSpaasPowerplant.Model
{
   
    public class Calculation
    {
        private PowerPlants myOrderedPowerPlants;
        private Fuels fuels;
        private int load;
        public Calculation(payload thePayload)
        {
            this.myOrderedPowerPlants = new PowerPlants(thePayload.powerplants);
            this.fuels = thePayload.fuels;
            this.load = thePayload.load; 
        }
        private IEnumerable<PowerLoad> GeneratePlan()
        {
            updateCosts(fuels);
            myOrderedPowerPlants.Sort();// uses the implemented CompareTo method to order by merit
            List<PowerLoad> productionPlan = new List<PowerLoad>();
            int matchedLoad = 0;
            int pMinTotal = 0;
            int delta = load;
            int deltaPMin = load;
            foreach(Powerplant powerplant in myOrderedPowerPlants) 
            {
                delta =  load - matchedLoad;
                deltaPMin = load - pMinTotal;
                if (delta > 0)
                {
                    float correctionIfWind = powerplant.type == "windturbine" ? (float) fuels.wind / 100 : 1;
                    int power = (int)Math.Min(delta, powerplant.pmax * correctionIfWind);
                    if (powerplant.pmin > deltaPMin)
                    {
                        power = 0; //not selected, too much PMin - optimization required
                                    // maybe need to unselect previous big gasfired 
                    } 
                    else
                    {
                    matchedLoad += power;
                    pMinTotal += powerplant.pmin;
                    }
                    productionPlan.Add(new PowerLoad(powerplant, power));
                }
                if (delta == 0)
                { //load is already fulfilled
                    productionPlan.Add(new PowerLoad(powerplant, 0));
                }
            }

            if (delta > 0)
            {
                // Cannot meet demand
            }
            if(pMinTotal > load)
            {
            }
            // can calculate all costs here and optimize
            return productionPlan;
            
        }

        private void updateCosts(Fuels fuels)
        {
            myOrderedPowerPlants.updateCosts(fuels);
        }

        public IEnumerable<PowerLoad> GetProductionPlan()
        {
            return GeneratePlan();
        }
    }
}
