namespace GemSpaasPowerplant.Model
{
    public class PowerplantComparer : IComparer<PowerPlant>
    {
        public int Compare(PowerPlant? x, PowerPlant? y)
        {
            if (x is null)
            {
                throw new ArgumentNullException("x");
            }
            return x.CompareTo(y);
        }
    }
}
