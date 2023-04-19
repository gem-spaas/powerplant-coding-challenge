using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProductionPlan.Models;

// ProductionRequestDto myDeserializedClass = JsonConvert.DeserializeObject<ProductionRequestDto>(myJsonResponse);
public class Fuels
{
    [JsonProperty("gas(euro/MWh)")]
    public double GasEuroMWh { get; set; }

    [JsonProperty("kerosine(euro/MWh)")]
    public double KerosineEuroMWh { get; set; }

    [JsonProperty("co2(euro/ton)")]
    public double Co2EuroTon { get; set; }

    [JsonProperty("wind(%)")]
    public int Wind { get; set; }
}

public class PowerPlant
{
    public string Name { get; set; }
    public PlantType Type { get; set; }
    public double Efficiency { get; set; }
    public int Pmin { get; set; }
    public int Pmax { get; set; }
}

public class ProductionRequestDto
{
    public int Load { get; set; }
    public Fuels Fuels { get; set; }
    public List<PowerPlant> Powerplants { get; set; }
}