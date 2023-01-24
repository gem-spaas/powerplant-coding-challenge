namespace PowerplantChallenge.Model
{
    public enum FuelType
    {
        Gaz,
        Kerosine,
        Wind,
        Co2
    }

    public class PowerplantType
    {
        public string TypeLib { get; set; }

        public bool GenerateCO2 { get; set; }

        public FuelType FuelType { get; set; }

        public static readonly PowerplantType[] SupportedTypes = new[]
        {
            new PowerplantType {
                TypeLib = "gasfired",
                GenerateCO2 = true,
                FuelType = FuelType.Gaz
            },
            new PowerplantType {
                TypeLib = "turbojet",
                GenerateCO2 = false,
                FuelType = FuelType.Kerosine
            },
            new PowerplantType {
                TypeLib = "windturbine",
                GenerateCO2 = false,
                FuelType = FuelType.Wind
            },
        };
    }
}