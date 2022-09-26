using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EngieApi.Controllers.Tests
{
    [TestClass()]
    public class ProductionPlanControllerTests
    {
        [TestMethod()]
        public void PostTestPayload1()
        {
            var controller = new ProductionPlanController(new NullLogger<ProductionPlanController>());
            string text = File.ReadAllText(@"..\..\..\example_payloads\payload1.json");
            using JsonDocument document = JsonDocument.Parse(text);
            var result = controller.Post(document.RootElement);
            int load = 0;
            using (var jsonDoc = JsonDocument.Parse(result.Value.ToString()))
            {
                foreach (var property in jsonDoc.RootElement.EnumerateArray())
                {
                    ProductionPlanResponse? productionPlanResponse =
                    JsonSerializer.Deserialize<ProductionPlanResponse>(property);
                    load += productionPlanResponse.P;
                }
            }
            Assert.AreEqual(document.RootElement.GetProperty("load").GetInt32(), load);
        }
        [TestMethod()]
        public void PostTestPayload2()
        {
            var controller = new ProductionPlanController(new NullLogger<ProductionPlanController>());
            string text = File.ReadAllText(@"..\..\..\example_payloads\payload2.json");
            using JsonDocument document = JsonDocument.Parse(text);
            var result = controller.Post(document.RootElement);
            int load = 0;
            using (var jsonDoc = JsonDocument.Parse(result.Value.ToString()))
            {
                foreach (var property in jsonDoc.RootElement.EnumerateArray())
                {
                    ProductionPlanResponse? productionPlanResponse =
                    JsonSerializer.Deserialize<ProductionPlanResponse>(property);
                    load += productionPlanResponse.P;
                }
            }
            Assert.AreEqual(document.RootElement.GetProperty("load").GetInt32(), load);
        }
        [TestMethod()]
        public void PostTestPayload3()
        {
            var controller = new ProductionPlanController(new NullLogger<ProductionPlanController>());
            string text = File.ReadAllText(@"..\..\..\example_payloads\payload3.json");
            using JsonDocument document = JsonDocument.Parse(text);
            var result = controller.Post(document.RootElement);
            int load = 0;
            using (var jsonDoc = JsonDocument.Parse(result.Value.ToString()))
            {
                foreach (var property in jsonDoc.RootElement.EnumerateArray())
                {
                    ProductionPlanResponse? productionPlanResponse =
                    JsonSerializer.Deserialize<ProductionPlanResponse>(property);
                    load += productionPlanResponse.P;
                }
            }
            Assert.AreEqual(document.RootElement.GetProperty("load").GetInt32(), load);
        }
    }
}