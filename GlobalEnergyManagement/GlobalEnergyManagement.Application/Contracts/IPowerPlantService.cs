using GlobalEnergyManagement.Application.DTOs;

namespace GlobalEnergyManagement.Application.Contracts;

public interface IPowerPlantService
{
    Task<ICollection<PowerPlantPower>> CalculateProductionPlan(PowerPlantPayload payload, bool includeCarbonOxide = false);
}