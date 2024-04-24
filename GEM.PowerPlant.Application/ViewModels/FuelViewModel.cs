using System.Text.Json.Serialization;

namespace GEM.PowerPlant.Application.ViewModels
{
	public class FuelViewModel
	{
		[JsonPropertyName("gas(euro/MWh)")]
		public double GasPrice { get; set; }

		[JsonPropertyName("kerosine(euro/MWh)")]
		public double KerosinePrice { get; set; }

		[JsonPropertyName("co2(euro/ton)")]
		public double CarbonEmissionTax { get; set; }

		[JsonPropertyName("wind(%)")]
		public double WindAvailaibilty { get; set; }
	}
}
