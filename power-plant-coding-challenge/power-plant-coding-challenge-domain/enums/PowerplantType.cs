using System.Text.Json.Serialization;

namespace power_plant_coding_challenge_domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PowerplantType
{
   
    Windturbine,
    Gasfired,
    Turbojet
}
