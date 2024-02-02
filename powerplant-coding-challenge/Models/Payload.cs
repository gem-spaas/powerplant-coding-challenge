using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace powerplant_coding_challenge.Models
{

    public class Payload
    {
        public int load { get; set; }
        public Fuels fuels { get; set; }
        public List<Powerplant> powerplants { get; set; }
    }
        
    public class Fuels
    {
        [JsonProperty("gas(euro/MWh)")]
        public double gaseuroMWh { get; set; }=1.0;

        [JsonProperty("kerosine(euro/MWh)")]
        public double kerosineeuroMWh { get; set; }=1.0;

        [JsonProperty("co2(euro/ton)")]
        public int co2euroton { get; set; }=1;

        [JsonProperty("wind(%)")]
        public int wind { get; set; }=100;
    }

    public class Powerplant
    {
        public string name { get; set; }
        public string type { get; set; }
        public double efficiency { get; set; }
        public int pmin { get; set; }
        public int pmax { get; set; }
    }

        
        
} 
    