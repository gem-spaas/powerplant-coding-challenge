using System.Text.Json.Serialization;

namespace GemSpaasPowerplant.Model
{
    public class PowerPlant : IComparable<PowerPlant>
    {
        public string name { get; set; }
        public string type { get; set; }
        public float efficiency { get; set; }
        public int pmin { get; set; }
        public int pmax { get; set; }
        public PowerPlant(PowerplantJsn powerplant)
        {
            this.name = powerplant.name;
            this.type = powerplant.type;
            this.efficiency = powerplant.efficiency;    
            this.pmin = powerplant.pmin;    
            this.pmax = powerplant.pmax;
            this.availablePMax = this.pmax;

        }
        public int availablePMax { get; set; }
        public float powerCost { get; set; }
        public float co2Cost { get; set; }
        public int p { get; set; }

        private float getMerit()
        {
            return this.powerCost;
        }
        /// <summary>
        /// Compares powerplants by their type, efficiency and pMin; allows to sort them
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(PowerPlant? other)
        {
            if (other == null) return -1; //this instance precedes the null
            //first sort on merit
            if (this.getMerit() < other.getMerit())
                return -1;
            if (this.getMerit() > other.getMerit())
                return 1;
            //sort on pmin
            if (this.pmin < other.pmin)
                return -1;
            if (this.pmin > other.pmin)
                return 1;
            return 0;
        }

        internal void updateCost(Fuels fuels)
        {
            switch (type)
            {
                case "windturbine":
                    this.powerCost = 0;
                    this.availablePMax = (int) (this.pmax * (float)fuels.wind / 100);
                    break;
                case "gasfired":
                    this.powerCost = (float)(fuels.gaseuroMWh + 0.3 * fuels.co2euroton) / efficiency;
                    break;
                case "turbojet":
                    this.powerCost = (float)(fuels.kerosineeuroMWh + 0.3 * fuels.co2euroton) / efficiency;
                    break;
                default:
                    throw new NotImplementedException($" type not defined {type}");
            }
        }
    }
}
