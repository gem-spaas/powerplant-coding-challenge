using Domain.Exceptions;

namespace Domain.Value_Objects
{
    public class Fuel
    {
        public double Gas { get; }
        public double Kerosine { get; }
        public double Co2 { get; }
        public double Wind { get; }

        private Fuel(
            double gas,
            double kerosine,
            double co2,
            double wind)
        {
            Gas = gas;
            Kerosine = kerosine;
            Co2 = co2;
            Wind = wind;
        }

        public static Fuel Create(
            double gas,
            double kerosine,
            double co2,
            double wind)
        {
            if (gas < 0)
                throw new InvalidFuelException("Gas price cannot be negative.");

            if (kerosine < 0)
                throw new InvalidFuelException("Kerosine price cannot be negative.");

            if (co2 < 0)
                throw new InvalidFuelException("CO2 price cannot be negative.");

            if (wind is < 0 or > 100)
                throw new InvalidFuelException(
                    "Wind percentage must be between 0 and 100.");

            return new Fuel(gas, kerosine, co2, wind);
        }
    }
}
