namespace powerplant.PowerPlantActivationStrategy
{
    public class RenewableEnergyStrategy : IPowerPlantActivationStrategy
    {
        public double GetProduction(double pmin, double pmax, double load, double windPercent = 0)
        {
            var canProduce = Math.Round(pmax * windPercent / 100, 2); 
            if (canProduce > load) { return load; }
            else { return canProduce; }
        }
    }
}
