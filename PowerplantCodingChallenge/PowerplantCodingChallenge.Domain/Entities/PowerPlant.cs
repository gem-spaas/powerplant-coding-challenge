using PowerplantCodingChallenge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowerplantCodingChallenge.Domain.Entities
{
    public class PowerPlant
    {
        public string Name { get; set; }
        public PowerPlantType Type { get; set; }
        public double Efficiency { get; set; }
        public int Pmin { get; set; }
        public int Pmax { get; set; }
    }
}
