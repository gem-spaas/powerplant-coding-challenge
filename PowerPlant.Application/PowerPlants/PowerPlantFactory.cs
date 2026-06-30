namespace PowerPlant.Application.PowerPlants;

public sealed class PowerPlantFactory : IPowerPlantFactory
{
    public Domain.PowerPlants.PowerPlant Create(string type, string name, decimal efficiency, decimal minPower, decimal maxPower)
    {
        return type switch
        {
            "gasfired" => new Domain.PowerPlants.GasFiredPlant(name, minPower, maxPower, efficiency),
            "turbojet" => new Domain.PowerPlants.TurbojetPlant(name, minPower, maxPower, efficiency),
            "windturbine" => new Domain.PowerPlants.WindTurbinePlant(name, maxPower),
            _ => throw new ArgumentException($"Unknown power plant type: {type}")
        };
    }
}
