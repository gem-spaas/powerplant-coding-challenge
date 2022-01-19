using System.Text.Json.Serialization;


namespace WebApplicationOssia.Models
{
    public class ProductionPlanItem
    {
        #region Public Methods
        public override string ToString()
        {
            return string.Concat("Name:  ", Name, "    Power:  ", P.ToString(), " MWh");
        }
        #endregion
        #region Public Properties
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("p")]
        public double P { get; set; }
        #endregion  
    }
}
