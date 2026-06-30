using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PowerPlant.Application.ProductionPlanning;

public record ProductionPlanRequest(
   decimal Load,
   FuelsDto Fuels,
   IReadOnlyList<PowerPlantDto> Powerplants);
public record FuelsDto(
   [property: JsonPropertyName("gas(euro/MWh)")] decimal Gas,
   [property: JsonPropertyName("kerosine(euro/MWh)")] decimal Kerosine,
   [property: JsonPropertyName("co2(euro/ton)")] decimal Co2,
   [property: JsonPropertyName("wind(%)")] decimal WindPercentage);

public record PowerPlantDto(
   string Name,
   string Type,
   decimal Efficiency,
   decimal Pmin,
   decimal Pmax);
