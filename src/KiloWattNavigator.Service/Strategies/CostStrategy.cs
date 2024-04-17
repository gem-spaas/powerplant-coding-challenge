using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiloWattNavigator.Service.Strategies
{
// Strategy: CostStrategy
class CostStrategy : IUnitCommitmentStrategy
{
    public void SolveUnitCommitment()
    {
        Console.WriteLine("Using CostStrategy");
        // CostStrategy logic for ON/OFF status of generating units
    }
}
}
