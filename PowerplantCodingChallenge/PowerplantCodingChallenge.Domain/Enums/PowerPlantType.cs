using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Domain.Enums;

/// <summary>
/// Available powerplant types
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<PowerPlantType>))]
public enum PowerPlantType
{
    /// <summary>Gas-fired powerplant</summary>
    gasfired,

    /// <summary>Kerosine-powered turbojet</summary>
    turbojet,

    /// <summary>Wind turbine</summary>
    windturbine
}
