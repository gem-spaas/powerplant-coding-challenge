namespace PowerPlant.Application.PowerPlants;

public interface IPowerPlantFactory
{
    Domain.PowerPlants.PowerPlant Create(string type, string name, decimal efficiency, decimal minPower, decimal maxPower);
}
