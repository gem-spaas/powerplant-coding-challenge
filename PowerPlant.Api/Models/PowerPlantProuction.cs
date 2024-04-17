using Newtonsoft.Json;
using System.ComponentModel;

namespace powerplant.Models
{
    public class PowerPlantProuction
    {

        [JsonProperty("name")]
        public string PowerPlantName { get; set; }
        [DefaultValue(0)]
        [JsonProperty("p")]
        public double Production { get; set; }
    }
}
