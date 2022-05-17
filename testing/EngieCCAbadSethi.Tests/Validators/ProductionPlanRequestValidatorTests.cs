using CalculatePowerGenerationByPowerPlants.Constants;
using CalculatePowerGenerationByPowerPlants.Models;
using CalculatePowerGenerationByPowerPlants.Validators;
using System;
using System.Collections.Generic;
using Xunit;

namespace CalculatePowerGeneratedByPlants.Tests.Validators
{
    public class ProductionPlanRequestValidatorTests
    {
        [Fact]
        public void Validate_WithValidRequest_WithNoException()
        {
            //arrange
            var fuels = new Dictionary<string, double>();
            fuels.Add("gas(euro/MWh)", 13.4);
            fuels.Add("kerosine(euro/MWh)", 50.8);
            fuels.Add("co2(euro/ton)", 20);
            fuels.Add("wind(%)", 60);

            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant{ Efficiency = 1 , Name = "wind1", Pmin = 0, Pmax = 150, Type = PowerPlantTypeConstants.WindTurbine},
                new PowerPlant{ Efficiency = 1 , Name = "wind2", Pmin = 0, Pmax = 65, Type = PowerPlantTypeConstants.WindTurbine}
            };

            var productionPlanRequest = new ProductionPlanRequest
            {
                Load = 90,
                Fuels = fuels,
                Powerplants = powerPlants
            };

            //act
            var exception = Record.Exception(() => new ProductionPlanRequestValidator().Validate(productionPlanRequest));

            //assert
            Assert.Null(exception);
        }

        [Fact]
        public void Validate_WithZeroLoad_ThrowsException()
        {
            //arrange
            var fuels = new Dictionary<string, double>();
            fuels.Add("gas(euro/MWh)", 13.4);
            fuels.Add("kerosine(euro/MWh)", 50.8);
            fuels.Add("co2(euro/ton)", 20);
            fuels.Add("wind(%)", 60);

            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant{ Efficiency = 1 , Name = "wind1", Pmin = 0, Pmax = 150, Type = PowerPlantTypeConstants.WindTurbine},
                new PowerPlant{ Efficiency = 1 , Name = "wind2", Pmin = 0, Pmax = 65, Type = PowerPlantTypeConstants.WindTurbine}
            };

            var productionPlanRequest = new ProductionPlanRequest
            {
                Load = 0,
                Fuels = fuels,
                Powerplants = powerPlants
            };

            //act
            Action action = () => new ProductionPlanRequestValidator().Validate(productionPlanRequest);

            //assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Validate_WithNoFuels_ThrowsException()
        {
            //arrange
            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant{ Efficiency = 1 , Name = "wind1", Pmin = 0, Pmax = 150, Type = PowerPlantTypeConstants.WindTurbine},
                new PowerPlant{ Efficiency = 1 , Name = "wind2", Pmin = 0, Pmax = 65, Type = PowerPlantTypeConstants.WindTurbine}
            };

            var productionPlanRequest = new ProductionPlanRequest
            {
                Load = 0,
                Powerplants = powerPlants
            };

            //act
            Action action = () => new ProductionPlanRequestValidator().Validate(productionPlanRequest);

            //assert
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Validate_WithMoreLoadThanCapability_ThrowsException()
        {
            //arrange
            var fuels = new Dictionary<string, double>();
            fuels.Add("gas(euro/MWh)", 13.4);
            fuels.Add("kerosine(euro/MWh)", 50.8);
            fuels.Add("co2(euro/ton)", 20);
            fuels.Add("wind(%)", 60);

            var powerPlants = new List<PowerPlant>
            {
                new PowerPlant{ Efficiency = 1 , Name = "wind1", Pmin = 0, Pmax = 150, Type = PowerPlantTypeConstants.WindTurbine},
                new PowerPlant{ Efficiency = 1 , Name = "wind2", Pmin = 0, Pmax = 65, Type = PowerPlantTypeConstants.WindTurbine}
            };

            var productionPlanRequest = new ProductionPlanRequest
            {
                Load = 300,
                Fuels = fuels,
                Powerplants = powerPlants
            };

            //act
            Action action = () => new ProductionPlanRequestValidator().Validate(productionPlanRequest);

            //assert
            Assert.Throws<ArgumentException>(action);
        }
    }
}
