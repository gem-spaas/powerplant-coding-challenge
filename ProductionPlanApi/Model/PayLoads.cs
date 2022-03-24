namespace ProductionPlanApi.Model;
using System.Text.Json;

public class Payloads
{
        public double? Load { get; set; }
        //public List<Fuel> Fuels {get; set;}
         public Fuels? Fuels { get; set; }
        public List<PowerPlant>? PowerPlants {get;set;}

        public override string ToString()
        { 
             string jsonString = JsonSerializer.Serialize(this);
            return jsonString;
        }        
}
