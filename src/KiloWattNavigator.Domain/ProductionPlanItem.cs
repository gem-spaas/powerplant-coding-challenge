using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KiloWattNavigator.Domain
{
    public class ProductionPlanResponseItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("p")]
        //[JsonConverter(typeof(DecimalFormatConverter))]
        public double P { get; set; }
    }

  
}
