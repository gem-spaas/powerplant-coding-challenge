using EngieApi;
using EngieApi.Processing;
using FluentAssertions;
using Newtonsoft.Json;

namespace ApiTests
{
    public class LoadPlanCalculatorFixture
    {
        [Theory]
        [InlineData("../../../payload/payload1.json")]
        [InlineData("../../../payload/payload2.json")]
        [InlineData("../../../payload/payload3.json")]
        [InlineData("../../../payload/payload4.json")]
        [InlineData("../../../payload/payload5.json")]
        [InlineData("../../../payload/payload6.json")]
        [InlineData("../../../payload/payload7.json")]
        [InlineData("../../../payload/payload8.json")]
        [InlineData("../../../payload/payload9.json")]
        public void GetLoadPlan_WhenCalled_ShouldReturnExecptedLoad(string payloadFile)
        {
            var payload = JsonConvert.DeserializeObject<ProductionPlanRequest>(File.ReadAllText(payloadFile));
            ILoadPlanCalculator loadPlanCalculator = new LoadPlanCalculator();
            var result = loadPlanCalculator.GetLoadPlan(payload);

            int actual = result.ProductionPlans.Sum(x => x.P);

            actual.Should().Be(payload.Load);

        }

        [Theory]
        [InlineData("../../../payload/payload1.json")]
        [InlineData("../../../payload/payload2.json")]
        [InlineData("../../../payload/payload3.json")]
        [InlineData("../../../payload/payload4.json")]
        [InlineData("../../../payload/payload5.json")]
        [InlineData("../../../payload/payload6.json")]
        [InlineData("../../../payload/payload7.json")]
        [InlineData("../../../payload/payload8.json")]
        [InlineData("../../../payload/payload9.json")]
        public void GetLoadPlan_WhenCalled_ShouldReturnAllPlansBetweenPMaxAndPMin(string payloadFile)
        {
            // Setup
            var payload = JsonConvert.DeserializeObject<ProductionPlanRequest>(File.ReadAllText(payloadFile));
            ILoadPlanCalculator loadPlanCalculator = new LoadPlanCalculator();
            var result = loadPlanCalculator.GetLoadPlan(payload);

            result.ProductionPlans.FindAll(c => c.P > 0).ForEach(x =>
            {
                x.P.Should().BeLessOrEqualTo(payload.PowerPlants.Find(p => p.Name == x.Name).PMax);
                x.P.Should().BeGreaterOrEqualTo(payload.PowerPlants.Find(p => p.Name == x.Name).PMin);
            });
        }


        [Theory]
        [InlineData("../../../payload/payload10.json")]

        public void GetLoadPlan_WhenNoValidPlan_ShouldReturnNull(string payloadFile)
        {
            // Setup
            var payload = JsonConvert.DeserializeObject<ProductionPlanRequest>(File.ReadAllText(payloadFile));
            ILoadPlanCalculator loadPlanCalculator = new LoadPlanCalculator();
            var result = loadPlanCalculator.GetLoadPlan(payload);

            result.Should().BeNull();
        }

    }
}