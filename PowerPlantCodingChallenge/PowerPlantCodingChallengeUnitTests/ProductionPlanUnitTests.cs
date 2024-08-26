using FluentAssertions;
using PowerPlantCodingChallenge.ProductionPlan;
using PowerPlantCodingChallenge.Services;

namespace PowerPlantCodingChallengeUnitTests
{
    public class ProductionPlanUnitTests
    {
        [Fact]
        public void GenerateProductionPlanSimpleTest()
        {
            // Arrange
            var forecast = new PowerLoadForecast()
            {
                Load = 910,
                Fuels = new Fuels
                {
                    GasCostPerMWh = 13.4F,
                    KerosineCostPerMWh = 50.8F,
                    Co2EmissionCostPerTon = 20F,
                    WindPercentage = 60F
                },
                PowerPlants = new PowerPlant[]
                {
                    new PowerPlant
                    {
                        Name = "gasfiredbig1",
                        PowerType = PowerPlantPowerType.GasFired,
                        Efficiency = 0.53F,
                        Pmin = 100F,
                        Pmax = 460F
                    },
                    new PowerPlant
                    {
                        Name = "gasfiredbig2",
                        PowerType = PowerPlantPowerType.GasFired,
                        Efficiency = 0.53F,
                        Pmin = 100F,
                        Pmax = 460F
                    },
                    new PowerPlant
                    {
                        Name = "gasfiredsomewhatsmaller",
                        PowerType = PowerPlantPowerType.GasFired,
                        Efficiency = 0.37F,
                        Pmin = 40F,
                        Pmax = 210F
                    },
                    new PowerPlant
                    {
                        Name = "tj1",
                        PowerType = PowerPlantPowerType.TurboJet,
                        Efficiency = 0.3F,
                        Pmin = 0F,
                        Pmax = 16F
                    },
                    new PowerPlant
                    {
                        Name = "windpark1",
                        PowerType = PowerPlantPowerType.WindTurbine,
                        Efficiency = 1F,
                        Pmin = 0F,
                        Pmax = 150F
                    },
                    new PowerPlant
                    {
                        Name = "windpark2",
                        PowerType = PowerPlantPowerType.WindTurbine,
                        Efficiency = 1F,
                        Pmin = 0F,
                        Pmax = 36F
                    },
                }
            };
            var expectedResult = new List<UnitCommitment>
            {
                new UnitCommitment
                {
                    Name = "windpark1",
                    Power = 90.0F
                },
                new UnitCommitment
                {
                    Name = "windpark2",
                    Power = 21.6F
                },
                new UnitCommitment
                {
                    Name = "gasfiredbig1",
                    Power = 460.0F
                },
                new UnitCommitment
                {
                    Name = "gasfiredbig2",
                    Power = 338.4F
                },
                new UnitCommitment
                {
                    Name = "gasfiredsomewhatsmaller",
                    Power = 0.0F
                },
                new UnitCommitment
                {
                    Name = "tj1",
                    Power = 0.0F
                },
            };
            var service = new ProductionPlanService();

            // Act
            var result = service.GenerateProductionPlan(forecast);

            // Assert
            expectedResult.Should().BeEquivalentTo(result);
        }
    }
}