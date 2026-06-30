using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Domain.Exceptions;

public sealed class InfeasibleLoadException(decimal load) : Exception($"No combination of power plants can satisfy a load of {load} MWh.")
{
    public decimal Load { get; } = load;
}