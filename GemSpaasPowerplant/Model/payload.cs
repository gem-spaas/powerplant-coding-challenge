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
        [JsonIgnore]
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
                    this.powerCost = (float) (fuels.gaseuroMWh + 0.3 * fuels.co2euroton) / efficiency;
                    break;
                case "turbojet":
                    this.powerCost = (float) (fuels.kerosineeuroMWh + 0.3 * fuels.co2euroton) / efficiency;
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
    public class PowerPlants
    {
        private List<Powerplant> myPowerPlants;

        public int Count => myPowerPlants.Count();


        public  PowerPlants(IEnumerable<Powerplant> pp)
        {
            this.myPowerPlants = pp.ToList();
        }

        internal void UpdateCosts(Fuels fuels)
        {
            this.myPowerPlants.ForEach(pp => pp.updateCost(fuels));
        }
        public int MatchedLoad()
        {
            return (int) this.myPowerPlants.Sum(l => l.p);
        }

        internal int PMinTotal()
        {
            return (int)this.myPowerPlants.Where(p=>p.p >0).Sum(l => l.pmin);
        }
        internal int PMaxTotal()
        {
            return (int)this.myPowerPlants.Where(p => p.p > 0).Sum(l => l.pmax);
        }

        internal void Sort()
        {
             myPowerPlants.Sort();
        }
        internal Powerplant GetPlant(int index)
        {
            return this.myPowerPlants[index];
        }

        

        internal IEnumerable<Powerplant> GetAll()
        {
            return myPowerPlants;
        }
    }
}
