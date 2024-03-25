namespace PowerPlantCC.Contracts.ProductionPlan.Models;

public record GetProductionPlanRequestDTO(
    int Load,
    FuelsDTO Fuels,
    PowerPlantDTO[] PowerPlants
);