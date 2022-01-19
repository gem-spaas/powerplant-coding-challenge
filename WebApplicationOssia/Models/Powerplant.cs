namespace WebApplicationOssia.Models
{
    public class Powerplant
    {
        #region Public Methods
        public override string ToString()
        {
            return string.Concat("Name:  ", Name, "    Type:  ", Type, "    Cost:  ", CostToGeneratePower.ToString(), " EUR");
        }
        #endregion
        #region Public Properties
        public string Name { get; set; }
        public string Type { get; set; }
        public double Efficiency { get; set; }
        public double Pmin { get; set; }
        public double Pmax { get; set; }

        public double CostToGeneratePower { get; set; }
        public double CostFuel { get; set; }
        public double CostCo2 { get; set; }
        public double Co2Emission { get; set; }
        public double Availabilty { get; set; }
        #endregion
    }
}
