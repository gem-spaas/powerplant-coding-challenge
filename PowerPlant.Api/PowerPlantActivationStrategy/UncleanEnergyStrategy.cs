namespace powerplant.PowerPlantActivationStrategy
{
    public class UncleanEnergyStrategy : IPowerPlantActivationStrategy
    {
        public double GetProduction(double pmin, double pmax, double load, double windPercent = 0)
        {
            if (load < pmin) { return pmin; }
            var canProduce = pmax; 
            if (canProduce > load) { return load; }
            else { return canProduce; }
        }
    }
}
