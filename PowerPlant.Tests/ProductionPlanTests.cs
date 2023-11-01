using System.Text.Json;
using FluentAssertions;
using PowerPlant.Models;
using PowerPlant.Services;

namespace PowerPlant.Tests;

public class ProductionPlanTests
{
    [Fact]
    public async Task GetProductionPlanShouldReturnACorrectProductionPlan()
    {

        var productionPlanService = new ProductionPlanService(new PowerPlantService());

        var payload = await File.ReadAllTextAsync(Path.Combine("example_payloads", "payload3.json"));

        var expectedProductionPlan = await File.ReadAllTextAsync(Path.Combine("example_payloads", "response3.json"));

        var requestedLoad = JsonSerializer.Deserialize<RequestedLoad>(payload);

        var productionPlan = productionPlanService.GetProductionPlan(requestedLoad);

        productionPlan.Should().BeEquivalentTo(JsonSerializer.Deserialize<IEnumerable<ProductionPlan>>(expectedProductionPlan));

    }

    [Fact]
    public async Task GetClosestPowerPlantShouldReturnTheClosestPminPowerPlant()
    {
        var productionPlanService = new ProductionPlanService(new PowerPlantService());

        var payload = await File.ReadAllTextAsync(Path.Combine("example_payloads", "payload2.json"));


        var requestedLoad = JsonSerializer.Deserialize<RequestedLoad>(payload);

        var closestPowerPlant = productionPlanService.GetClosestPowerPlant(requestedLoad.Powerplants,20);

         closestPowerPlant.Name.Should().Be("gasfiredsomewhatsmaller");

    }
}