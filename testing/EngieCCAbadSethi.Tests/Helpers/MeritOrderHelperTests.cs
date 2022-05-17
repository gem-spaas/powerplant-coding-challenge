using CalculatePowerGenerationByPowerPlants.Constants;
using CalculatePowerGenerationByPowerPlants.Helpers;
using CalculatePowerGenerationByPowerPlants.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CalculatePowerGeneratedByPlants.Tests.Helpers
{
    public class MeritOrderHelperTests
    {
        [Fact]
        public void CalculateProductionPlan_ExamplePayload1_Success()
        {
            //arrange
            var expected = new List<ProductionPlanResponse>
            {
                new ProductionPlanResponse{Name = "windpark1", P = 90},
                new ProductionPlanResponse{Name = "windpark1", P = 21},
                new ProductionPlanResponse{Name = "gasfiredbig1", P = 369}
            };

            var fuels = new Dictionary<string, double>();
            fuels.Add("gas(euro/MWh)", 13.4);
            fuels.Add("kerosine(euro/MWh)", 50.8);
            fuels.Add("co2(euro/ton)", 20);
            fuels.Add("wind(%)", 60);

            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant{ Efficiency = 0.53 , Name = "gasfiredbig1", Pmin = 100, Pmax = 460, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.53 , Name = "gasfiredbig2", Pmin = 100, Pmax = 460, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.37 , Name = "gasfiredsomewhatsmaller", Pmin = 40, Pmax = 210, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.3 , Name = "tj1", Pmin = 0, Pmax = 16, Type = PowerPlantTypeConstants.Turbojet},
                new PowerPlant{ Efficiency = 1 , Name = "windpark1", Pmin = 0, Pmax = 150, Type = PowerPlantTypeConstants.WindTurbine},
                new PowerPlant{ Efficiency = 1 , Name = "windpark2", Pmin = 0, Pmax = 36, Type = PowerPlantTypeConstants.WindTurbine}
            };

            var productionPlanRequest = new ProductionPlanRequest
            {
                Load = 480,
                Fuels = fuels,
                Powerplants = powerPlants
            };

            //act
            var actual = MeritOrderHelper.CalculateProductionPlanByMeritOrder(productionPlanRequest);

            //assert
            var result1 = expected.Except(actual, ProductionPlanResponseListComparer.Instance);
            var result2 = actual.Except(expected, ProductionPlanResponseListComparer.Instance);
            Assert.Equal(result1.Any(), result2.Any());
        }

        [Fact]
        public void CalculateProductionPlan_ExamplePayload2_Success()
        {
            //arrange
            var expected = new List<ProductionPlanResponse>
            {
                new ProductionPlanResponse{Name = "gasfiredbig1", P = 380},
                new ProductionPlanResponse{Name = "gasfiredbig1", P = 100}
            };

            var fuels = new Dictionary<string, double>();
            fuels.Add("gas(euro/MWh)", 13.4);
            fuels.Add("kerosine(euro/MWh)", 50.8);
            fuels.Add("co2(euro/ton)", 20);
            fuels.Add("wind(%)", 0);

            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant{ Efficiency = 0.53 , Name = "gasfiredbig1", Pmin = 100, Pmax = 460, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.53 , Name = "gasfiredbig2", Pmin = 100, Pmax = 460, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.37 , Name = "gasfiredsomewhatsmaller", Pmin = 40, Pmax = 210, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.3 , Name = "tj1", Pmin = 0, Pmax = 16, Type = PowerPlantTypeConstants.Turbojet},
                new PowerPlant{ Efficiency = 1 , Name = "windpark1", Pmin = 0, Pmax = 150, Type = PowerPlantTypeConstants.WindTurbine},
                new PowerPlant{ Efficiency = 1 , Name = "windpark2", Pmin = 0, Pmax = 36, Type = PowerPlantTypeConstants.WindTurbine}
            };

            var productionPlanRequest = new ProductionPlanRequest
            {
                Load = 480,
                Fuels = fuels,
                Powerplants = powerPlants
            };

            //act
            var actual = MeritOrderHelper.CalculateProductionPlanByMeritOrder(productionPlanRequest);

            //assert
            var result1 = expected.Except(actual, ProductionPlanResponseListComparer.Instance);
            var result2 = actual.Except(expected, ProductionPlanResponseListComparer.Instance);
            Assert.Equal(result1.Any(), result2.Any());
        }

        [Fact]
        public void CalculateProductionPlan_ExamplePayload3_Success()
        {
            //arrange
            var expected = new List<ProductionPlanResponse>
            {
                new ProductionPlanResponse{Name = "windpark1", P = 90},
                new ProductionPlanResponse{Name = "windpark1", P = 21},
                new ProductionPlanResponse{Name = "gasfiredbig1", P = 460},
                new ProductionPlanResponse{Name = "gasfiredbig1", P = 339}
            };

            var fuels = new Dictionary<string, double>();
            fuels.Add("gas(euro/MWh)", 13.4);
            fuels.Add("kerosine(euro/MWh)", 50.8);
            fuels.Add("co2(euro/ton)", 20);
            fuels.Add("wind(%)", 60);

            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant{ Efficiency = 0.53 , Name = "gasfiredbig1", Pmin = 100, Pmax = 460, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.53 , Name = "gasfiredbig2", Pmin = 100, Pmax = 460, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.37 , Name = "gasfiredsomewhatsmaller", Pmin = 40, Pmax = 210, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.3 , Name = "tj1", Pmin = 0, Pmax = 16, Type = PowerPlantTypeConstants.Turbojet},
                new PowerPlant{ Efficiency = 1 , Name = "windpark1", Pmin = 0, Pmax = 150, Type = PowerPlantTypeConstants.WindTurbine},
                new PowerPlant{ Efficiency = 1 , Name = "windpark2", Pmin = 0, Pmax = 36, Type = PowerPlantTypeConstants.WindTurbine}
            };

            var productionPlanRequest = new ProductionPlanRequest
            {
                Load = 910,
                Fuels = fuels,
                Powerplants = powerPlants
            };

            //act
            var actual = MeritOrderHelper.CalculateProductionPlanByMeritOrder(productionPlanRequest);

            //assert
            var result1 = expected.Except(actual, ProductionPlanResponseListComparer.Instance);
            var result2 = actual.Except(expected, ProductionPlanResponseListComparer.Instance);
            Assert.Equal(result1.Any(), result2.Any());
        }

        [Fact]
        public void CalculateProductionPlan_InvalidPayload3_Error()
        {
            //arrange
            var expected = new List<ProductionPlanResponse>
            {
                new ProductionPlanResponse{Name = "windpark1", P = 90},
                new ProductionPlanResponse{Name = "windpark1", P = 21},
                new ProductionPlanResponse{Name = "gasfiredbig1", P = 460},
                new ProductionPlanResponse{Name = "gasfiredbig1", P = 339}
            };

            var fuels = new Dictionary<string, double>();
            fuels.Add("kerosine(euro/MWh)", 50.8);
            fuels.Add("co2(euro/ton)", 20);
            fuels.Add("wind(%)", 60);

            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant{ Efficiency = 0.53 , Name = "gasfiredbig1", Pmin = 100, Pmax = 460, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.53 , Name = "gasfiredbig2", Pmin = 100, Pmax = 460, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.37 , Name = "gasfiredsomewhatsmaller", Pmin = 40, Pmax = 210, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.3 , Name = "tj1", Pmin = 0, Pmax = 16, Type = PowerPlantTypeConstants.Turbojet},
                new PowerPlant{ Efficiency = 1 , Name = "windpark1", Pmin = 0, Pmax = 150, Type = PowerPlantTypeConstants.WindTurbine},
                new PowerPlant{ Efficiency = 1 , Name = "windpark2", Pmin = 0, Pmax = 36, Type = PowerPlantTypeConstants.WindTurbine}
            };

            var productionPlanRequest = new ProductionPlanRequest
            {
                Load = 910,
                Fuels = fuels,
                Powerplants = powerPlants
            };

            //act
            Action action = () => MeritOrderHelper.CalculateProductionPlanByMeritOrder(productionPlanRequest);

            //assert//Assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void CalculateProductionPlan_InvalidExamplePayload2_Error()
        {
            //arrange
            var expected = new List<ProductionPlanResponse>
            {
                new ProductionPlanResponse{Name = "gasfiredbig1", P = 380},
                new ProductionPlanResponse{Name = "gasfiredbig1", P = 100}
            };

            var fuels = new Dictionary<string, double>();
            fuels.Add("gas(euro/MWh)", 13.4);
            fuels.Add("co2(euro/ton)", 20);
            fuels.Add("wind(%)", 0);

            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant{ Efficiency = 0.53 , Name = "gasfiredbig1", Pmin = 100, Pmax = 460, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.53 , Name = "gasfiredbig2", Pmin = 100, Pmax = 460, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.37 , Name = "gasfiredsomewhatsmaller", Pmin = 40, Pmax = 210, Type = PowerPlantTypeConstants.GasFired},
                new PowerPlant{ Efficiency = 0.3 , Name = "tj1", Pmin = 0, Pmax = 16, Type = PowerPlantTypeConstants.Turbojet},
                new PowerPlant{ Efficiency = 1 , Name = "windpark1", Pmin = 0, Pmax = 150, Type = PowerPlantTypeConstants.WindTurbine},
                new PowerPlant{ Efficiency = 1 , Name = "windpark2", Pmin = 0, Pmax = 36, Type = PowerPlantTypeConstants.WindTurbine}
            };

            var productionPlanRequest = new ProductionPlanRequest
            {
                Load = 480,
                Fuels = fuels,
                Powerplants = powerPlants
            };

            //act
            Action action = () => MeritOrderHelper.CalculateProductionPlanByMeritOrder(productionPlanRequest);

            //assert//Assert
            Assert.Throws<ArgumentException>(action);
        }
    }
}