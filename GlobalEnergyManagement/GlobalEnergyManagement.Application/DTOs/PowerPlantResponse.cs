namespace GlobalEnergyManagement.Application.DTOs;

public record PowerPlantResponse(ICollection<PowerPlantPower> PowerPlantPowers);