using powerplant_coding_challenge.Infrastructure.Models;

namespace powerplant_coding_challenge.Domain
{
    public interface IPowerClient
    {
        /// <summary>
        /// Calculates the utilization of individual power plants based on energy demand.
        /// </summary>
        /// <param name="productionLoad">PowerPlant utilization request</param>
        /// <returns>PowerPlant utilization plan</returns>
        public Task<ProductionPlan?> GetPowerPlan (ProductionLoad productionLoad);
    }
}