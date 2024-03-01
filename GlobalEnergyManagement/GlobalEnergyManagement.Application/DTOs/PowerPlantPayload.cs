namespace GlobalEnergyManagement.Application.DTOs;

public record PowerPlantPayload(
    int Load,
    PowerPlantFuels Fuels,
    ICollection<PowerPlants> PowerPlants
);