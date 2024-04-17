using KiloWattNavigator.Service.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiloWattNavigator.Service
{
    class UnitCommitmentSolver
    {
        private IUnitCommitmentStrategy _strategy;

        public UnitCommitmentSolver(IUnitCommitmentStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Solve()
        {
            _strategy.SolveUnitCommitment();
        }
    }
}
