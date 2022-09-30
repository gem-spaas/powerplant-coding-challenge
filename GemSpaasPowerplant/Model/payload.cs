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
        public PowerplantJsn[] powerplants { get; set; }
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

    public class PowerplantJsn 
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
       
    }
}
