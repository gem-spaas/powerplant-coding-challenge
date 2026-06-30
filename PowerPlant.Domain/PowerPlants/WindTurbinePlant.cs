using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Domain.PowerPlants;

public sealed class WindTurbinePlant(string name, decimal maxPower) : PowerPlant(name, 0m, maxPower)
{
    public override decimal CostPerMwh(Fuels fuels) => 0m;
    public override decimal EffectiveMaximum(Fuels fuels) => MaxPower * fuels.WindPercentage / 100m;
    public override decimal EffectiveMinimum(Fuels fuels) => EffectiveMaximum(fuels);
}
