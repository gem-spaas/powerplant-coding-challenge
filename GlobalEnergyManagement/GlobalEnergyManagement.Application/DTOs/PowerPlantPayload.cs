namespace GlobalEnergyManagement.Application.DTOs;

public record PowerPlantPayload(int Load, ICollection<PowerPlantFuels> Fuels, ICollection<PowerPlants> PowerPlants);