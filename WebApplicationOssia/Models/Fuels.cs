using System.Text.Json.Serialization;

namespace WebApplicationOssia.Models
{
    public class Fuels
    {
        #region Public Methods
        public override string ToString()
        {
            return string.Concat("Fuels:   "
                , "Gas ", Gas.ToString(), " euro/MWh  -  "
                , "Kerosine ", Kerosine.ToString(), " euro/MWh  -  "
                , "CO2 ", Co2.ToString(), " euro/ton  -  "
                , "Wind ", Wind.ToString(), " % ");
        }
        #endregion
        #region Public Properties
        [JsonPropertyName("gas(euro/MWh)")]
        public double Gas { get; set; }

        [JsonPropertyName("kerosine(euro/MWh)")]
        public double Kerosine { get; set; }

        [JsonPropertyName("co2(euro/ton)")]
        public double Co2 { get; set; }

        [JsonPropertyName("wind(%)")]
        public double Wind { get; set; }
        #endregion
    }
}
