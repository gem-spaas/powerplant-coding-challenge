using EngieApi;
using EngieApi.Processing;
using Newtonsoft.Json;

namespace ApiTests
{
    public class ProductionPlanTests
    {
        [Theory]
        [InlineData("../../../payload/payload1.json")]
        [InlineData("../../../payload/payload2.json")]
        [InlineData("../../../payload/payload3.json")]
        public void TestPayloads_WhenLoadMustBeTheSameWithCalculatedLoad(string payloadFile)
        {
            var payload = JsonConvert.DeserializeObject<ProductionPlanRequest>(File.ReadAllText(payloadFile));
            var result = Calculator.GetLoadPlan(payload);
            int load = 0;

            foreach (var productionPlan in result.ProductionPlans)
            {
                load += productionPlan.P;
            }
            Assert.Equal(payload.Load, load);
        }
    }
}