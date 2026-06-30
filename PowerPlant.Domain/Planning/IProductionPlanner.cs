using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Domain.Planning;

public interface IProductionPlanner
{
    IReadOnlyList<PowerOutput> Plan(decimal load, IReadOnlyList<Domain.PowerPlants.PowerPlant> plants, Fuels fuels);
}
