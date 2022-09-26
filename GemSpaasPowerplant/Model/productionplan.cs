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

        public PowerLoad(Powerplant powerplant, int p)
        {
            this.Powerplant = powerplant;
            this.name = powerplant.name;
            this.p = p;
        }
        public Powerplant getPowerPlant() { return Powerplant; }
    }

}
