using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Domain.PowerPlants;

public sealed class TurbojetPlant(string name, decimal minPower, decimal maxPower, decimal efficiency) : PowerPlant(name, minPower, maxPower)
{
    public override decimal CostPerMwh(Fuels fuels) => fuels.KerosinePricePerMwh / efficiency;
}
