namespace Gem.PowerplantChallenge.Service.Models;

public class TurboJetPowerplant : Powerplant
{
    public TurboJetPowerplant(string name, double efficiency, double pMin, double pMax, double cost) : base(name, efficiency, pMin, pMax, cost)
    {
    }
}