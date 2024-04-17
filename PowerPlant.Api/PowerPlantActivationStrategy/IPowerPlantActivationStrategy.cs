namespace powerplant.PowerPlantActivationStrategy
{
    public interface IPowerPlantActivationStrategy
    {
        public double GetProduction(double pmin, double pmax, double load, double windPercent =0);
    }
}
