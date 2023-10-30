using System.Text.Json;
using FluentAssertions;
using PowerPlant.Models;
using PowerPlant.Services;

namespace PowerPlant.Tests
{
    public class UnitTest1
    {

        [Fact]
        public async Task GetProductionPlanShouldReturnACorrectProductionPlan()
        {
            var productionPlanService = new ProductionPlanService();

            var payload = await File.ReadAllTextAsync(Path.Combine("example_payloads", "payload3.json"));

            var expectedProductionPlan = await File.ReadAllTextAsync(Path.Combine("example_payloads", "response3.json"));

            var requestedLoad = JsonSerializer.Deserialize<RequestedLoad>(payload);

            var productionPlan =await productionPlanService.GetProductionPlan(requestedLoad);
            
           productionPlan.Should().BeEquivalentTo(JsonSerializer.Deserialize<IEnumerable<ProductionPlan>>(expectedProductionPlan));
            
        }

        [Fact]
        public async Task OrdeByMertiShouldReturnWindFirst()
        {
            var productionPlanService = new ProductionPlanService();

            var payload = await File.ReadAllTextAsync(Path.Combine("example_payloads", "payload3.json"));

            var requestedLoad = JsonSerializer.Deserialize<RequestedLoad>(payload);

            var orderByMerit = productionPlanService.OrderByMerit(requestedLoad.Powerplants, requestedLoad.Fuels);
         
            orderByMerit.First().Type.Should().Be(PowerPlantType.windturbine.ToString());
        }
    }
}