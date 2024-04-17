using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using powerplant.Controllers;
using powerplant.Helper;
using powerplant.Models;
using powerplant.Service;

namespace PowerPlantTests
{
    public class PowerPlantServiceTests
    {

        [Test]
        public void WhenWindPercentMoreThanZeor_ShouldActivateWindPowerPlant()
        {
            //Act
            var powerPlantService = new PowerPlantService();
            var productionPlanRequest =  new PowerPlantRequest
            {
                Load = 480,
                Fuels = new Fuels
                {
                    GasPrice = 13.4,
                    KerosinePrice = 50.8,
                    Co2euroton = 20,
                    Wind = 60
                },
                PowerPlants =
                [
                    new() { Name = "gasfiredbig1", Type = PowerPlantType.gasfired.ToString(), Efficiency = 0.53, Pmin = 100, Pmax = 460 },
                    new() { Name = "gasfiredbig2", Type = PowerPlantType.gasfired.ToString(), Efficiency = 0.53, Pmin = 100, Pmax = 460 },
                    new() { Name = "gasfiredsomewhatsmaller", Type = PowerPlantType.gasfired.ToString(), Efficiency = 0.37, Pmin = 40, Pmax = 210 },
                    new() { Name = "tj1", Type = PowerPlantType.turbojet.ToString(), Efficiency = 0.3, Pmin = 0, Pmax = 16 },
                    new() { Name = "windpark1", Type = PowerPlantType.windturbine.ToString(), Efficiency = 1, Pmin = 0, Pmax = 150 },
                    new() { Name = "windpark2", Type = PowerPlantType.windturbine.ToString(), Efficiency = 1, Pmin = 0, Pmax = 36 }
                ]
             };
            //Act
            var result = powerPlantService.ActivatePowerPlants(productionPlanRequest);
            //Ass
            var windPowerPlant = result.First(p => p.PowerPlantName == "windpark1");
            Assert.AreEqual(90, windPowerPlant.Production);
        }
        [Test]
        public void WhenWindPercentnZerO_ShouldActivateGasPowerPlantFirst()
        {
            //Act
            var powerPlantService = new PowerPlantService();
            var productionPlanRequest = new PowerPlantRequest
            {
                Load = 480,
                Fuels = new Fuels
                {
                    GasPrice = 13.4,
                    KerosinePrice = 50.8,
                    Co2euroton = 20,
                    Wind = 0
                },
                PowerPlants =
                [
                    new() { Name = "gasfiredbig1", Type = PowerPlantType.gasfired.ToString(), Efficiency = 0.53, Pmin = 100, Pmax = 460 },
                    new() { Name = "gasfiredbig2", Type = PowerPlantType.gasfired.ToString(), Efficiency = 0.53, Pmin = 100, Pmax = 460 },
                    new() { Name = "gasfiredsomewhatsmaller", Type = PowerPlantType.gasfired.ToString(), Efficiency = 0.37, Pmin = 40, Pmax = 210 },
                    new() { Name = "tj1", Type = PowerPlantType.turbojet.ToString(), Efficiency = 0.3, Pmin = 0, Pmax = 16 },
                    new() { Name = "windpark1", Type = PowerPlantType.windturbine.ToString(), Efficiency = 1, Pmin = 0, Pmax = 150 },
                    new() { Name = "windpark2", Type = PowerPlantType.windturbine.ToString(), Efficiency = 1, Pmin = 0, Pmax = 36 }
                ]
            };
            //Act
            var result = powerPlantService.ActivatePowerPlants(productionPlanRequest);
            //Ass
            var gasPowerPlant = result.First(p => p.PowerPlantName == "gasfiredbig1");

            Assert.AreEqual(460, gasPowerPlant.Production);
        }

        [Test]
        public void WhenNoWindAndGas_ShouldActivatekerosinePowerPlantFirst()
        {
            //Act
            var powerPlantService = new PowerPlantService();
            var productionPlanRequest = new PowerPlantRequest
            {
                Load = 480,
                Fuels = new Fuels
                {
                    GasPrice = 13.4,
                    KerosinePrice = 50.8,
                    Co2euroton = 20,
                    Wind = 0
                },
                PowerPlants =
                [
                    new() { Name = "tj1", Type = PowerPlantType.turbojet.ToString(), Efficiency = 0.3, Pmin = 0, Pmax = 16 },
                    new() { Name = "windpark1", Type = PowerPlantType.windturbine.ToString(), Efficiency = 1, Pmin = 0, Pmax = 150 },
                    new() { Name = "windpark2", Type = PowerPlantType.windturbine.ToString(), Efficiency = 1, Pmin = 0, Pmax = 36 }
                ]
            };
            //Act
            var result = powerPlantService.ActivatePowerPlants(productionPlanRequest);
            //Ass
            var windPowerPlant = result.First(p => p.PowerPlantName == "windpark1");
            var turbojetPowerPlant = result.First(p => p.PowerPlantName == "tj1");

            Assert.AreEqual(0, windPowerPlant.Production);
            Assert.AreEqual(16, turbojetPowerPlant.Production);
        }
    }
}