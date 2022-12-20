namespace powerplant_coding_challenge_implementation.Models
{
    public class ProductionPlanPayload
    {
        public int Load { get; set; }
        public Fuels Fuels { get; set; }
        public IEnumerable<PowerPlant> Powerplants { get; set; }
    }
}