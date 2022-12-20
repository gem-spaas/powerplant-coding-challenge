namespace powerplant_coding_challenge_implementation.Models
{
    public class ProductionPlanPayload
    {
        public int Load { get; set; }
        public Fuels Fuels { get; set; }
        public IEnumerable<PowerPlant> Powerplants { get; set; }

        public ProductionPlanPayload(int load, Fuels fuels, IEnumerable<PowerPlant> powerplants)
        {
            Load=load;
            Fuels=fuels;
            Powerplants=powerplants;
        }
    }
}