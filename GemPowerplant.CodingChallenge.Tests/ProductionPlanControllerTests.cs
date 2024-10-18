using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using GemPowerplant.CodingChallenge.Controllers;
using GemPowerplant.CodingChallenge.Models;
using GemPowerplant.CodingChallenge.Services;

namespace GemPowerplant.CodingChallenge.Tests
{
    public class ProductionPlanControllerTests
    {
        private readonly Mock<ILogger<ProductionPlanController>> _loggerMock;
        private readonly Mock<IProductionPlanService> _serviceMock;
        private readonly ProductionPlanController _controller;

        public ProductionPlanControllerTests()
        {
            _loggerMock = new Mock<ILogger<ProductionPlanController>>();
            _serviceMock = new Mock<IProductionPlanService>();
            _controller = new ProductionPlanController(_loggerMock.Object, _serviceMock.Object);
        }

        [Fact]
        public void CalculateProductionPlan_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new ProductionPlanRequest
            {
                Load = 100,
                Fuels = new Fuels(),
                Powerplants = new List<Powerplant>()
            };
            var expectedPlan = new List<ProductionPlanResponse>();

            _serviceMock.Setup(x => x.CalculateProductionPlan(
                request.Load,
                request.Fuels,
                request.Powerplants))
                .Returns(expectedPlan);

            // Act
            var result = _controller.CalculateProductionPlan(request);

            // Assert
            Assert.IsType<ActionResult<IEnumerable<ProductionPlanResponse>>>(result);
        }

        [Fact]
        public void CalculateProductionPlan_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Load", "Required");
            var request = new ProductionPlanRequest();

            // Act
            var result = _controller.CalculateProductionPlan(request);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }
    }
}