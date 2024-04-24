using System.Text.Json.Serialization;

namespace GEM.PowerPlant.Application.ViewModels
{
	public class PowerplantResponseViewModel
	{
		public string Name { get; set; }
		[JsonPropertyName("p")]
		public double DeliveredPower { get; set; }
	}
}
