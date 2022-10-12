using GemSpaasPowerplant.Controllers;
using GemSpaasPowerplant.Model;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace TestPowerPlantAPI
{
    [TestClass]
    public class ProductionPlanControllerTest
    {
        [TestMethod(), Timeout(500)]
        [DataRow("payload1", 13506.8)]
        [DataRow("payload2", 17569.8)]
        [DataRow("payload3", 29246.4)]
        [DataRow("payload4", 3660.4)]
        public void TestPayload1(string payloadFile, double expectedMinimumCost)
        {
            ProductionPlanController controller = new (new NullLogger<ProductionPlanController>(), new Calculation());
            string payloadString = File.ReadAllText($"..\\..\\..\\..\\example_payloads\\{payloadFile}.json");

            payload payload = JsonSerializer.Deserialize<payload>(payloadString);

            IEnumerable<PowerLoad> productionPlan = controller.Post(payload);
            PowerPlants powerPlants = new PowerPlants(payload.powerplants);
            powerPlants.UpdateCosts(payload.fuels);

            int totalPower = 0;
                foreach (PowerLoad powerload in productionPlan)
                {
                string name = powerload.name;
                int p = powerload.p;
                PowerPlant powerPlant = powerPlants.GetPlant(name);
                powerPlant.p = p;
                Assert.IsTrue(p>= powerPlant.pmin || p==0, $"power pmin {p} < {powerPlant.pmin}  constraint not respected for {powerPlant.name}" );
                Assert.IsTrue(p <= powerPlant.pmax, $"power pmax{p} > {powerPlant.pmax}  constraint not respected for {powerPlant.name}" );

                totalPower += p;
                }

            Assert.AreEqual(expectedMinimumCost, powerPlants.RunningCost(), 0.1);
            Assert.AreEqual(payload.load, totalPower, "Load not matched");
        }
    }
}