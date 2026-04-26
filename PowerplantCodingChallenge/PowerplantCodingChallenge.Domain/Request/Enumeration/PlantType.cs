using System.Text.Json.Serialization;

namespace PowerplantCodingChallenge.Domain.Request.Enumeration
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PlantType
    {
        [JsonStringEnumMemberName("gasfired")] GasFired = 0,
        [JsonStringEnumMemberName("turbojet")] TurboJet = 1,
        [JsonStringEnumMemberName("windturbine")] WindTurbine = 2
    }
}
