
using System.ComponentModel;

namespace powerplant.Models
{
    public class Powerplant
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Efficiency { get; set; }
        public int Pmin { get; set; }
        public int Pmax { get; set; }

        [DefaultValue(0)]
        public double Cost { get; set; }

    }
}
