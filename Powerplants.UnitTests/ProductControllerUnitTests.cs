using BusinessLogic.Services.Calculation;
using Microsoft.AspNetCore.Mvc;
using Powerplant.Controllers;


namespace PowerPlant.UnitTests
{
    public class ProductControllerUnitTests
    {
        private readonly ProductController _controller;
        private readonly ICalculationPowerPlanService _calculatorPowerPlanService;

        public ProductControllerUnitTests()
        {
            _calculatorPowerPlanService = new CalculationPowerPlanService();
            _controller = new ProductController(_calculatorPowerPlanService);
        }
        [Fact]
        public void Type_Response_ShouldBe_Ok()
        {
            var payload = HelperMethods.GetPayload();

            IActionResult result = _controller.CalculatePowerPlants(payload);
            Assert.IsType<OkObjectResult>(result);
        }


    }
}