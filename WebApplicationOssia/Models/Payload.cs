namespace WebApplicationOssia.Models
{
    public class Payload
    {
        #region Public Properties
        public double Load { get; set; }
        public Fuels Fuels { get; set; }
        public List<Powerplant> PowerPlants { get; set; }
        #endregion
    }
}
