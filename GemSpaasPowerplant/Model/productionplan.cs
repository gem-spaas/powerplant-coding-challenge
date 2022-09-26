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
        private Powerplant Powerplant { get; set; }

        public PowerLoad(Powerplant powerplant)
        {
            this.Powerplant = powerplant;
            this.name = powerplant.name;
            this.p = powerplant.p;
        }
        public Powerplant getPowerPlant() { return Powerplant; }
    }

}
