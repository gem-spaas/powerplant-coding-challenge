namespace ProductionPlanApi.Model;


public class PowerPlant
{
    public static readonly  string[] types =
    {
        "gasfired", "turbojet", "windturbine",
        //in some other electrical system they may use also
         "watertubine", "nuclearplant", "solarfield"
    };
       

    public string? Name { get; set;}
    public string? Type { get; set; }
    public double? Pmin { get; set; }
    public double? Pmax { get; set; }
    public double? Efficiency{ get; set; }
 
}
