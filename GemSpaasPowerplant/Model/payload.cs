using System.Collections;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace GemSpaasPowerplant.Model
{
    public class payload
    {
        /// <example>480</example>
        public int load { get; set; }
        public Fuels fuels { get; set; }
        public Powerplant[] powerplants { get; set; }
    }

    public class Fuels
    {
        /// <example>13.4</example>
        [JsonPropertyName("gas(euro/MWh)")]
        public float gaseuroMWh { get; set; }
        /// <example>50.8</example>
        [JsonPropertyName("kerosine(euro/MWh)")]
        public float kerosineeuroMWh { get; set; }
        /// <example>20</example>
        [JsonPropertyName("co2(euro/ton)")]
        public int co2euroton { get; set; }
        /// <example>60</example>
        [JsonPropertyName("wind(%)")]

        public int wind { get; set; }
    }

    public class Powerplant : IComparable<Powerplant>
    {
        /// <example>gasfiredbig1</example>
        [JsonPropertyName("name")]
        public string name { get; set; }
        /// <example>gasfired</example>
        [JsonPropertyName("type")]
        public string type { get; set; }
        /// <example>0.53</example>

        [JsonPropertyName("efficiency")]
        public float efficiency { get; set; }
        /// <example>100</example>
        [JsonPropertyName("pmin")]
        public int pmin { get; set; }
        /// <example>460</example>
        [JsonPropertyName("pmax")]
        public int pmax { get; set; }
        /// <example>0</example>
        [JsonIgnore]

        public float powerCost { get; set; }
        [JsonIgnore]
        public float co2Cost { get; set; }

        private float getMerit()
        {
            return this.powerCost;
    }
        /// <summary>
        /// Compares powerplants by their type, efficiency and pMin; allows to sort them
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Powerplant? other)
        {
            if (other==null) return -1; //this instance precedes the null
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
                    break;
                case "gasfired":
                    this.powerCost = (float) fuels.gaseuroMWh / efficiency;
                    break;
                case "turbojet":
                    this.powerCost = (float) fuels.kerosineeuroMWh / efficiency;
                    break;
                default:
                    throw new NotImplementedException($" type not defined {type}");
            }
        }
    }
    public class PowerplantComparer : IComparer<Powerplant>
    {
        public int Compare(Powerplant? x, Powerplant? y)
        {
            if (x is null)
            {
                throw new ArgumentNullException("x");
            }
            return x.CompareTo(y);
        }
    }
    public class PowerPlants : IEnumerable<Powerplant>
    {
        private List<Powerplant> myPowerPlants;

        public  PowerPlants(IEnumerable<Powerplant> pp)
        {
            this.myPowerPlants = pp.ToList();
        }
        public IEnumerator<Powerplant> GetEnumerator()
        {
            return myPowerPlants.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return myPowerPlants.GetEnumerator();
        }
        public void Sort()
        {
            myPowerPlants.Sort();
        }

        internal void UpdateCosts(Fuels fuels)
        {
            this.myPowerPlants.ForEach(pp => pp.updateCost(fuels));
        }
    }
}
