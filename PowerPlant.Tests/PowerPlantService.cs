using System.Text.Json;
using FluentAssertions;
using PowerPlant.Models;
using PowerPlant.Services;

namespace PowerPlant.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task OrdeByMertiShouldReturnWindFirst()
        {

            var powerPlantService = new PowerPlantService();

            var payload = await File.ReadAllTextAsync(Path.Combine("example_payloads", "payload3.json"));

            var requestedLoad = JsonSerializer.Deserialize<RequestedLoad>(payload);

            var orderByMerit = powerPlantService.OrderByMerit(requestedLoad.Powerplants, requestedLoad.Fuels);
         
            orderByMerit.First().Type.Should().Be(PowerPlantType.windturbine.ToString());
        }
    }
}