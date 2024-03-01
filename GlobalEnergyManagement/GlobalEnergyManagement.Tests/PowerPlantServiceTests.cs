using FluentAssertions;
using GlobalEnergyManagement.Application.DTOs;
using GlobalEnergyManagement.Application.Services;

namespace GlobalEnergyManagement.Tests;

public class PowerPlantServiceTests
{
    [Fact]
    public async Task CalculateProductionPlan_ShouldReturn_SuccessAndCorrectResult()
    {
        // Arrange

        var powerPlantService = new PowerPlantService();

        var examplePayload = new PowerPlantPayload(
            910,
            new PowerPlantFuels(13.4, 50.8, 20, 60),
            new List<PowerPlants>()
            {
                new("gasfiredbig1", "gasfired", 0.53, 100, 460, 0),
                new("gasfiredbig2", "gasfired", 0.53, 100, 460, 0),
                new("gasfiredsomewhatsmaller", "gasfired", 0.37, 40, 210, 0),
                new("tj1", "turbojet", 0.3, 0, 16, 0),
                new("windpark1", "windturbine", 1, 0, 150, 0),
                new("windpark2", "windturbine", 1, 0, 36, 0),
                
            });

        var expectedResponse = new List<PowerPlantPower>()
        {
            new("windpark1", 90.0, 0),
            new("windpark2", 21.6, 0),
            new("gasfiredbig1", 460.0, 0),
            new("gasfiredbig2", 338.4, 0),
            new("gasfiredsomewhatsmaller", 0.0, 0),
            new("tj1", 0.0, 0),
        };

        // Act

        var resultPowers = await powerPlantService.CalculateProductionPlan(examplePayload, false);
        
        // Assert

        resultPowers.Should().NotBeNullOrEmpty();
        resultPowers.Count.Should().Be(expectedResponse.Count);

        foreach (var resultPower in resultPowers)
        {
            var expectedResult = expectedResponse.Find(d => d.Name == resultPower.Name);

            expectedResult.Should().NotBeNull();

            resultPower.Name.Should().Be(expectedResult!.Name);
            resultPower.Power.Should().Be(expectedResult!.Power);
        }
    }
    
    [Fact]
    public async Task CalculateProductionPlanWithCarbonOxide_ShouldReturn_SuccessAndCorrectResult()
    {
        // Arrange

        var powerPlantService = new PowerPlantService();

        var examplePayload = new PowerPlantPayload(
            910,
            new PowerPlantFuels(13.4, 50.8, 20, 60),
            new List<PowerPlants>()
            {
                new("gasfiredbig1", "gasfired", 0.53, 100, 460, 0),
                new("gasfiredbig2", "gasfired", 0.53, 100, 460, 0),
                new("gasfiredsomewhatsmaller", "gasfired", 0.37, 40, 210, 0),
                new("tj1", "turbojet", 0.3, 0, 16, 0),
                new("windpark1", "windturbine", 1, 0, 150, 0),
                new("windpark2", "windturbine", 1, 0, 36, 0),
                
            });

        var expectedResponse = new List<PowerPlantPower>()
        {
            new("windpark1", 90.0, 0),
            new("windpark2", 21.6, 0),
            new("gasfiredbig1", 460.0, 0),
            new("gasfiredbig2", 338.4, 0),
            new("gasfiredsomewhatsmaller", 0.0, 0),
            new("tj1", 0.0, 0),
        };

        // Act

        var resultPowers = await powerPlantService.CalculateProductionPlan(examplePayload, true);
        
        // Assert

        resultPowers.Should().NotBeNullOrEmpty();
        resultPowers.Count.Should().Be(expectedResponse.Count);

        foreach (var resultPower in resultPowers)
        {
            var expectedResult = expectedResponse.Find(d => d.Name == resultPower.Name);

            expectedResult.Should().NotBeNull();

            resultPower.Name.Should().Be(expectedResult!.Name);
            resultPower.Power.Should().Be(expectedResult!.Power);
        }
    }

    
}