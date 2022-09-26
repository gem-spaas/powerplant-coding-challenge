using GemSpaasPowerplant.Controllers;
using GemSpaasPowerplant.Model;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace TestPowerPlantAPI
{
    [TestClass]
    public class ProductionPlanControllerTest
    {
        [TestMethod(), Timeout(100)]
        [DataRow("payload1")]
        [DataRow("payload2")]
        [DataRow("payload3")]
        public void TestPayload1(string payloadFile)
        {
            ProductionPlanController controller = new (new NullLogger<ProductionPlanController>());
            string payloadString = File.ReadAllText($"..\\..\\..\\..\\example_payloads\\{payloadFile}.json");

            payload payload = JsonSerializer.Deserialize<payload>(payloadString);

            IEnumerable<PowerLoad> productionPlan = controller.Post(payload);
            int totalPower = 0;
                foreach (PowerLoad powerload in productionPlan)
                {
                string name = powerload.name;
                int p = powerload.p;
                Powerplant powerplant = payload.powerplants.Where(p => p.name == name).First();
                Assert.IsTrue(p>= powerplant.pmin || p==0, $"power pmin {p} < {powerplant.pmin}  constraint not respected for {powerplant.name}" );
                Assert.IsTrue(p <= powerplant.pmax, $"power pmax{p} > {powerplant.pmax}  constraint not respected for {powerplant.name}" );
                totalPower += p;
                }
            
            Assert.AreEqual(payload.load, totalPower, "Load not matched");
        }
    }
}