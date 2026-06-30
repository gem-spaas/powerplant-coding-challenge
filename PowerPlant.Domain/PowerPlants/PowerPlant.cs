using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Domain.PowerPlants;

public abstract class PowerPlant(string name, decimal minPower, decimal maxPower)
{
    public string Name { get; } = name;
    public decimal MinPower { get; } = minPower;
    public decimal MaxPower { get; } = maxPower;

    public abstract decimal CostPerMwh(Fuels fuels);

    public virtual decimal EffectiveMinimum(Fuels fuels) => MinPower;
    public virtual decimal EffectiveMaximum(Fuels fuels) => MaxPower;
}
