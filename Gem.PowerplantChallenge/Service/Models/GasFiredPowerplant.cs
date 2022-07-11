namespace Gem.PowerplantChallenge.Service.Models;

public class GasFiredPowerplant: Powerplant
{
    public GasFiredPowerplant(string name, double efficiency, double pMin, double pMax, double cost) : base(name, efficiency, pMin, pMax, cost)
    {
    }
}