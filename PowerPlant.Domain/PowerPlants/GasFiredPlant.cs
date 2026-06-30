using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Domain.PowerPlants;

public sealed class GasFiredPlant(string name, decimal minPower, decimal maxPower, decimal efficiency) : PowerPlant(name, minPower, maxPower)
{
    private const decimal Co2EmissionPerMwh = 0.3m;
    public override decimal CostPerMwh(Fuels fuels)
    {
        var fuelCost = fuels.GasPricePerMwh / efficiency;
        var co2Cost = Co2EmissionPerMwh * fuels.Co2PricePerTon;
        return fuelCost + co2Cost;
    }
}
