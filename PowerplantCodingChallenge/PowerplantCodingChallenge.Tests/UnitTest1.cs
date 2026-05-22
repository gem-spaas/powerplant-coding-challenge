using PowerplantCodingChallenge.Application.DTOs;
using PowerplantCodingChallenge.Application.Services;
using Xunit;

namespace PowerplantCodingChallenge.Tests;

public class ProductionPlanServiceTests
{
    [Fact]
    public void CalculateProductionPlan_WithPayload3_ReturnsExpectedTotal()
    {
        var request = new ProductionPlanRequest
        {
            Load = 910,
            Fuels = new MarketDataDto
            {
                GasPrice = 13.4,
                KerosinePrice = 50.8,
                Co2Price = 20,
                WindPercentage = 60
            },
            Powerplants = new List<PowerPlantDto>
            {
                new PowerPlantDto { Name = "gasfiredbig1", Type = PowerplantCodingChallenge.Domain.Enums.PowerPlantType.gasfired, Efficiency = 0.53, Pmin = 100, Pmax = 460 },
                new PowerPlantDto { Name = "gasfiredbig2", Type = PowerplantCodingChallenge.Domain.Enums.PowerPlantType.gasfired, Efficiency = 0.53, Pmin = 100, Pmax = 460 },
                new PowerPlantDto { Name = "gasfiredsomewhatsmaller", Type = PowerplantCodingChallenge.Domain.Enums.PowerPlantType.gasfired, Efficiency = 0.37, Pmin = 40, Pmax = 210 },
                new PowerPlantDto { Name = "tj1", Type = PowerplantCodingChallenge.Domain.Enums.PowerPlantType.turbojet, Efficiency = 0.3, Pmin = 0, Pmax = 16 },
                new PowerPlantDto { Name = "windpark1", Type = PowerplantCodingChallenge.Domain.Enums.PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 150 },
                new PowerPlantDto { Name = "windpark2", Type = PowerplantCodingChallenge.Domain.Enums.PowerPlantType.windturbine, Efficiency = 1, Pmin = 0, Pmax = 36 }
            }
        };

        var service = new ProductionPlanService();
        var result = service.CalculateProductionPlan(request);

        var expected = new Dictionary<string, double>
        {
            ["windpark1"] = 90.0,
            ["windpark2"] = 21.6,
            ["gasfiredbig1"] = 460.0,
            ["gasfiredbig2"] = 338.4,
            ["gasfiredsomewhatsmaller"] = 0.0,
            ["tj1"] = 0.0
        };

        var byName = result.ToDictionary(r => r.Name, r => r.P);

        foreach (var kv in expected)
        {
            Assert.True(byName.ContainsKey(kv.Key), $"Missing plant {kv.Key} in result");
            Assert.Equal(kv.Value, byName[kv.Key], 1);
        }
    }
}
