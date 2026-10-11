using Domain.Enums;
using Domain.Exceptions;
using Domain.Value_Objects;

namespace Domain.Entities
{
    public class PowerPlant
    {
        public string Name { get; }        
        public double Efficiency { get; }

        public PlantType Type { get; }
        public double Pmin { get; }
        public double Pmax { get; }

        private PowerPlant(
        string name,
        double efficiency,
        PlantType type,
        double pmin,
        double pmax)
        {
            Name = name;
            Efficiency = efficiency;
            Type = type;
            Pmin = pmin;
            Pmax = pmax;
        }

        public static PowerPlant Create(
            string name,
            double efficiency,
            string type,
            double pmin,
            double pmax)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidPowerplantException("Name is required.");

            if (efficiency <= 0 || efficiency > 1)
                throw new InvalidPowerplantException(
                    "Efficiency must be between 0 and 1.");

            if (!Enum.TryParse<PlantType>(type, true, out var plantType))
                throw new InvalidPowerplantException(
                    $"Invalid plant type: {type}.");

            if (pmin < 0 || pmax < pmin)
                throw new InvalidPowerplantException(
                    "Invalid power limits.");

            return new PowerPlant(name, efficiency, plantType, pmin, pmax);
        }

        public double CalculateMerit(Fuel fuels) => Type switch
        {
            PlantType.WindTurbine => 0,

            PlantType.GasFired =>
                fuels.Gas / Efficiency,

            PlantType.Turbojet =>
                fuels.Kerosine / Efficiency,

            _ => throw new ArgumentOutOfRangeException(
                nameof(Type),
                Type,
                "Unsupported power plant type.")
        };
    }
}
