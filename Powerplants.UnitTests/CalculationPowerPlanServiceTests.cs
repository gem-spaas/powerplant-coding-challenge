using BusinessLogic.Services.Calculation;

namespace PowerPlant.UnitTests
{
    public class CalculationPowerPlanServiceTests
    {
        private readonly ICalculationPowerPlanService _calculatorPowerPlanService;

        public CalculationPowerPlanServiceTests()
        {
            _calculatorPowerPlanService = new CalculationPowerPlanService();
        }
        [Fact]
        public void Response_Should_Equals_Json_Response()
        {
            var payload = HelperMethods.GetPayload();
            payload.Fuels.GasEuroPerMWh = 13.4m;
            payload.Fuels.KerosineEuroPerMWh = 50.8m;
            payload.Fuels.Co2EuroPerTon = 20;
            payload.Fuels.WindPercentage = 60;

            var responseFromFile = HelperMethods.GetResponse();

            var result = _calculatorPowerPlanService.CalculatePowerPlants(payload);
            Assert.Equal(responseFromFile.Count, result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                responseFromFile[i].Name = result[i].Name;
                responseFromFile[i].P = result[i].P;
            }
        }


    }
}