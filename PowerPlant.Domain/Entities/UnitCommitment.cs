namespace PowerPlant.Domain.Entities;

public class UnitCommitment
{
    public string Name { get; private set; }
    public decimal Power { get; private set; }

    public UnitCommitment(
        string name,
        decimal power)
    {
        Name = name;
        Power = power;
    }

    public void AdjustPower(decimal newPower)
    {
        Power = newPower;
    }
}