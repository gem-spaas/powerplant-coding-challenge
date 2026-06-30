using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Domain;

public record Fuels(decimal GasPricePerMwh, decimal KerosinePricePerMwh, decimal Co2PricePerTon, decimal WindPercentage);
