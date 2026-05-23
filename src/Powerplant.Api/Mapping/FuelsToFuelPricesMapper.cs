using System.Globalization;
using AutoMapper;
using Powerplant.Api.Domain;
using Powerplant.Api.Generated;

namespace Powerplant.Api.Mapping;

public sealed class FuelsToFuelPricesMapper : ITypeConverter<Fuels, FuelPrices>
{
    public FuelPrices Convert(Fuels source, FuelPrices destination, ResolutionContext context)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return new FuelPrices
        {
            GasEuroPerMWh = GetRequiredDecimal(source, "gas(euro/MWh)"),
            KerosineEuroPerMWh = GetRequiredDecimal(source, "kerosine(euro/MWh)"),
            Co2EuroPerTon = GetRequiredDecimal(source, "co2(euro/ton)"),
            WindPercentage = GetRequiredDecimal(source, "wind(%)")
        };
    }

    private static decimal GetRequiredDecimal(
        Dictionary<string, decimal> properties,
        string key)
    {
        if (!properties.TryGetValue(key, out var value))
        {
            throw new ArgumentException($"Missing required fuel value '{key}'.");
        }

        return value;
    }

}
