namespace GemSpaasPowerplant.Model
{
    public class ProductionPlan
    {
        public PowerLoad[] powerload { get; set; }
    }

    public class PowerLoad
    {
        public string name { get; set; }
        public int p { get; set; }
        private PowerPlant Powerplant { get; set; }

        public PowerLoad(PowerPlant powerplant)
        {
            this.Powerplant = powerplant;
            this.name = powerplant.name;
            this.p = powerplant.p;
        }
        public PowerPlant getPowerPlant() { return Powerplant; }
    }

}
