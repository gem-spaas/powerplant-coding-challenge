using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KiloWattNavigator.Domain
{
    public class Powerplant
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("efficiency")]
        public double Efficiency { get; set; }
        [JsonPropertyName("pmax")]
        public double Pmax { get; set; }
        [JsonPropertyName("pmin")]
        public double Pmin { get; set; }

        private double Cost { get; set; }

        public double GetCost(Fuels fuels)
        {
            if (Type == "windturbine")
            {
                return 0; // Wind turbines have zero cost
            }
            else
            {
                Cost = (Type == "gasfired" ? fuels.Gas : fuels.Kerosine) / Efficiency; //Cost goes up when less efficient
                return Cost;
            }                
        }

        public double GetPowerMax(double availableWind)
        {
            var power = Pmax;
            ///Todo: Will PMin be affected as windmills need a min of power to run
            if (Type == "windturbine")
            {
                power =  Pmax * (availableWind/100);
            }

            return Math.Round(power * 10) / 10;
        }
    }
}
