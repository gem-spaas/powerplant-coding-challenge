using BusinessLogic.Models;
namespace BusinessLogic.Services.Calculation
{
    public interface ICalculationPowerPlanService
    {
        List<ProductResponse> CalculatePowerPlants(ProductPlanInput dto);
    }
}
