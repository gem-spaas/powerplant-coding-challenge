using GemPowerplant.CodingChallenge.Models;
using GemPowerplant.CodingChallenge.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemPowerplant.CodingChallenge.Tests
{
    public class ProductionPlanServiceTests
    {
        private readonly IProductionPlanService _sut;

        public ProductionPlanServiceTests()
        {
            _sut = new ProductionPlanService();
        }

        [Fact]
        public void CalculateProductionPlan_WithValidInput_ShouldReturnCorrectPlan()
        {
            // Arrange
            var load = 480.0;
            var fuels = new Fuels
            {
                Gas = 13.4,
                Kerosine = 50.8,
                Co2 = 20,
                Wind = 60
            };

            var powerplants = new List<Powerplant>
            {
                new() { Name = "windpark1", Type = PowerplantType.Windturbine, Efficiency = 1, Pmin = 0, Pmax = 150 },
                new() { Name = "windpark2", Type = PowerplantType.Windturbine, Efficiency = 1, Pmin = 0, Pmax = 36 },
                new() { Name = "gasfiredbig1", Type = PowerplantType.Gasfired, Efficiency = 0.53, Pmin = 100, Pmax = 460 }
            };

            // Act
            var result = _sut.CalculateProductionPlan(load, fuels, powerplants);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(load, result.Sum(r => r.P));
        }
    }
}
