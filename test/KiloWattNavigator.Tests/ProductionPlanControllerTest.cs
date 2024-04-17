using KiloWattNavigator.Controllers;
using KiloWattNavigator.Domain;
using KiloWattNavigator.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using System.Text.Json;


namespace KiloWattNavigator.Tests
{
    public class ProductionPlanControllerTest
    {

        private readonly ILogger<PowerPlantController> _logging = Substitute.For<ILogger<PowerPlantController>>();
        public ProductionPlanControllerTest()
        {
        }
        
        [Fact]
        public void Test_SuccessCall()
        {

            string jsonResult = "[{\"name\":\"windpark1\",\"p\":90.0},{\"name\":\"windpark2\",\"p\":21.6},{\"name\":\"gasfiredbig1\",\"p\":460.0},{\"name\":\"gasfiredbig2\",\"p\":338.4},{\"name\":\"gasfiredsomewhatsmaller\",\"p\":0.0},{\"name\":\"tj1\",\"p\": 0.0}\r\n]";
            string jsonPayload = "{\r\n  \"load\": 910,\r\n  \"fuels\":\r\n  {\r\n    \"gas(euro/MWh)\": 13.4,\r\n    \"kerosine(euro/MWh)\": 50.8,\r\n    \"co2(euro/ton)\": 20,\r\n    \"wind(%)\": 60\r\n  },\r\n  \"powerplants\": [\r\n    {\r\n      \"name\": \"gasfiredbig1\",\r\n      \"type\": \"gasfired\",\r\n      \"efficiency\": 0.53,\r\n      \"pmin\": 100,\r\n      \"pmax\": 460\r\n    },\r\n    {\r\n      \"name\": \"gasfiredbig2\",\r\n      \"type\": \"gasfired\",\r\n      \"efficiency\": 0.53,\r\n      \"pmin\": 100,\r\n      \"pmax\": 460\r\n    },\r\n    {\r\n      \"name\": \"gasfiredsomewhatsmaller\",\r\n      \"type\": \"gasfired\",\r\n      \"efficiency\": 0.37,\r\n      \"pmin\": 40,\r\n      \"pmax\": 210\r\n    },\r\n    {\r\n      \"name\": \"tj1\",\r\n      \"type\": \"turbojet\",\r\n      \"efficiency\": 0.3,\r\n      \"pmin\": 0,\r\n      \"pmax\": 16\r\n    },\r\n    {\r\n      \"name\": \"windpark1\",\r\n      \"type\": \"windturbine\",\r\n      \"efficiency\": 1,\r\n      \"pmin\": 0,\r\n      \"pmax\": 150\r\n    },\r\n    {\r\n      \"name\": \"windpark2\",\r\n      \"type\": \"windturbine\",\r\n      \"efficiency\": 1,\r\n      \"pmin\": 0,\r\n      \"pmax\": 36\r\n    }\r\n  ]\r\n}";
            ProductionPlanRequest deserializedProduct = System.Text.Json.JsonSerializer.Deserialize<ProductionPlanRequest>(jsonPayload);
            var productionPlanService = new ProductionPlanService();
            var target = new PowerPlantController(_logging, productionPlanService);

            var result = target.CalculateProductionPlan(deserializedProduct);
            var okResult = result as OkObjectResult;

            List<ProductionPlanResponseItem> productionPlanResponse = JsonConvert.DeserializeObject<List<ProductionPlanResponseItem>>(jsonResult);
            var powerPlants = productionPlanResponse?.ToList();
            var totalLoad = powerPlants.Sum(s => s.P);

            // Assert
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(6, powerPlants.Count);
        }

        [Fact]
        public void Load_Test()
        {
            // Arrange
            string jsonExpectedResult = "[{\"name\":\"windpark1\",\"p\":90.0},{\"name\":\"windpark2\",\"p\":21.6},{\"name\":\"gasfiredbig1\",\"p\":460.0},{\"name\":\"gasfiredbig2\",\"p\":338.4},{\"name\":\"gasfiredsomewhatsmaller\",\"p\":0.0},{\"name\":\"tj1\",\"p\": 0.0}\r\n]";
            string jsonPayload = "{\r\n  \"load\": 910,\r\n  \"fuels\":\r\n  {\r\n    \"gas(euro/MWh)\": 13.4,\r\n    \"kerosine(euro/MWh)\": 50.8,\r\n    \"co2(euro/ton)\": 20,\r\n    \"wind(%)\": 60\r\n  },\r\n  \"powerplants\": [\r\n    {\r\n      \"name\": \"gasfiredbig1\",\r\n      \"type\": \"gasfired\",\r\n      \"efficiency\": 0.53,\r\n      \"pmin\": 100,\r\n      \"pmax\": 460\r\n    },\r\n    {\r\n      \"name\": \"gasfiredbig2\",\r\n      \"type\": \"gasfired\",\r\n      \"efficiency\": 0.53,\r\n      \"pmin\": 100,\r\n      \"pmax\": 460\r\n    },\r\n    {\r\n      \"name\": \"gasfiredsomewhatsmaller\",\r\n      \"type\": \"gasfired\",\r\n      \"efficiency\": 0.37,\r\n      \"pmin\": 40,\r\n      \"pmax\": 210\r\n    },\r\n    {\r\n      \"name\": \"tj1\",\r\n      \"type\": \"turbojet\",\r\n      \"efficiency\": 0.3,\r\n      \"pmin\": 0,\r\n      \"pmax\": 16\r\n    },\r\n    {\r\n      \"name\": \"windpark1\",\r\n      \"type\": \"windturbine\",\r\n      \"efficiency\": 1,\r\n      \"pmin\": 0,\r\n      \"pmax\": 150\r\n    },\r\n    {\r\n      \"name\": \"windpark2\",\r\n      \"type\": \"windturbine\",\r\n      \"efficiency\": 1,\r\n      \"pmin\": 0,\r\n      \"pmax\": 36\r\n    }\r\n  ]\r\n}";
            ProductionPlanRequest deserializedPayLoad = System.Text.Json.JsonSerializer.Deserialize<ProductionPlanRequest>(jsonPayload);
            var productionPlanService = new ProductionPlanService();
            var target = new PowerPlantController(_logging,productionPlanService);

            // Act
            var result = target.CalculateProductionPlan(deserializedPayLoad);
            var okResult = result as OkObjectResult;

            List<ProductionPlanResponseItem> productionPlanExpectedResponse = JsonConvert.DeserializeObject<List<ProductionPlanResponseItem>>(jsonExpectedResult);
            List<ProductionPlanResponseItem>? productionPlanResponse = okResult.Value as List<ProductionPlanResponseItem>;
            var powerPlants = productionPlanResponse?.ToList();

            // Assert
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(deserializedPayLoad.Load, productionPlanResponse.Sum(s => s.P)); //should be the same value as the payload
            Assert.Equal(productionPlanExpectedResponse.Sum(s => s.P), productionPlanResponse.Sum(s => s.P)); //should be the same value as the proposed response
        }
    }  
}