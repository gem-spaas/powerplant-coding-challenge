namespace ProductionPlanApi.Model;

public class PowerPlantResponseItem
{

    public PowerPlantResponseItem(string name,double p)
    {
        this.Name = name;
        this.P=p;
    }
    public string? Name { get; set;}
    public double? P { get; set; }
}