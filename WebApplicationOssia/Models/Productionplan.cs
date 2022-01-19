using System.Text.Json.Serialization;


namespace WebApplicationOssia.Models
{
    public class ProductionPlan
    {
        #region Public Properties
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("productionplanitems")]
        public List<ProductionPlanItem> ProductionPlanItems { get; set; }
        [JsonPropertyName("result")]
        public string Result { get; set; }
        #endregion
    }
}
