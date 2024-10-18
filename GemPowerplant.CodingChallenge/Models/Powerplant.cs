using System.Text.Json.Serialization;

namespace GemPowerplant.CodingChallenge.Models
{
    public class Powerplant
    {
        public string? Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter<PowerplantType>))]
        public PowerplantType Type { get; set; }
        public double Efficiency { get; set; }
        public double Pmax { get; set; }
        public double Pmin { get; set; }
        public double Cost { get; set; }
    }

    public enum PowerplantType
    {
        Gasfired,
        Turbojet,
        Windturbine
    }
}
