using GlobalEnergyManagement.Application.Contracts;
using GlobalEnergyManagement.Application.DTOs;

namespace GlobalEnergyManagement.Application.Services;

public class PowerPlantService : IPowerPlantService
{
    public Task<ICollection<PowerPlantPower>> CalculateProductionPlan(PowerPlantPayload payload)
    {
        throw new NotImplementedException();
    }
}