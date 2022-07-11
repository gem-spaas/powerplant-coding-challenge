namespace Gem.PowerplantChallenge.Service.Models;

public class WindTurbinePowerplant: Powerplant
{
    public WindTurbinePowerplant(string name, double efficiency, double pMin, double pMax, double windPercentage) : base(name, efficiency, pMin, pMax, 0)
    {
        PMax = pMax * windPercentage / 100;
        PMin = pMax;
    }
}