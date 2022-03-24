namespace ProductionPlanApi.Model;

public class Fuels
{
        public double? gas { get; set; }
        public double? kerosine { get; set; }
        
        public double? co2 { get; set; }
        public double? wind { get; set; }

        public override string ToString()
        {
            return "gas(euro/MWh) " +" : "+gas +
                   "\n kerosine(euro/MWh) " +" : "+kerosine+
                   "\n co2 (euro/ton)" +" : "+co2+
                   " wind(%)" +" : "+wind;
        }        
}
