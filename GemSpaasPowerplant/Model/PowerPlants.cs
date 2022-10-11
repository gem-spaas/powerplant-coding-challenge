namespace GemSpaasPowerplant.Model
{
    public class PowerPlants
    {
        private List<PowerPlant> myPowerPlants;

        public int Count => myPowerPlants.Count();


        public  PowerPlants(IEnumerable<PowerplantJsn> pp)
        {
            this.myPowerPlants = pp.Select(pp => new PowerPlant(pp)).ToList();
        }


        public void UpdateCosts(Fuels fuels)
        {
            this.myPowerPlants.ForEach(pp => pp.updateCost(fuels));
        }
        public int MatchedLoad()
        {
            return (int) this.myPowerPlants.Sum(l => l.p);
        }

        internal int PMinTotal()
        {
            return (int)this.myPowerPlants.Where(p=>p.p >0).Sum(l => l.pmin);
        }
        internal int PMaxTotal()
        {
            return (int)this.myPowerPlants.Where(p => p.p > 0).Sum(l => l.pmax);
        }

        internal void Sort()
        {
             myPowerPlants.Sort();
        }
        internal PowerPlant GetPlant(int index)
        {
            return this.myPowerPlants[index];
        }
        public PowerPlant GetPlant(string name)
        {
            return this.myPowerPlants.Where(pp=>pp.name == name).First();
        }

        public double RunningCost()
        {
            return (double)this.myPowerPlants.Sum(l => l.powerCost*l.p);
        }
        

        internal IEnumerable<PowerPlant> GetAll()
        {
            return myPowerPlants;
        }
    }
}
