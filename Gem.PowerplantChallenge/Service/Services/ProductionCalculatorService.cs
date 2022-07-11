using Gem.PowerplantChallenge.Service.DTOs;
using Gem.PowerplantChallenge.Service.Models;

namespace Gem.PowerplantChallenge.Service.Services;

public interface IProductionCalculatorService
{
    List<PowerplantProductionDTO> GeneratePowerplantUsage(PowerplantsWithLoadDTO payload);
}

public class ProductionCalculatorService : IProductionCalculatorService
{
    public List<PowerplantProductionDTO> GeneratePowerplantUsage(PowerplantsWithLoadDTO payload)
    {
        var powerplantsAvailable = BuildPowerplantList(payload);

        powerplantsAvailable = powerplantsAvailable.OrderBy(x => x.Cost).ToList();

        var loadNeeded = payload.Load;
        foreach (var powerplant in powerplantsAvailable)
        {
            if (loadNeeded <= 0)
                break;

            powerplant.SetPActualBasedOnNeeds(loadNeeded);
            loadNeeded -= powerplant.PActual;

        }

        return BuildPowerplantUsage(powerplantsAvailable).Powerplants;
    }

    private List<Powerplant> BuildPowerplantList(PowerplantsWithLoadDTO payload)
    {
        var powerplantsAvailable = new List<Powerplant>();
        foreach (var powerplant in payload.Powerplants)
        {
            switch (powerplant.Type)
            {
                case ("windturbine"):
                    powerplantsAvailable.Add(new WindTurbinePowerplant(powerplant.Name, powerplant.Efficiency,
                        powerplant.PMin, powerplant.PMax, payload.Fuels.WindPercentage));
                    break;
                case ("gasfired"):
                    powerplantsAvailable.Add(new GasFiredPowerplant(powerplant.Name, powerplant.Efficiency,
                        powerplant.PMin, powerplant.PMax, payload.Fuels.GasPrice));
                    break;
                case ("turbojet"):
                    powerplantsAvailable.Add(new TurboJetPowerplant(powerplant.Name, powerplant.Efficiency,
                        powerplant.PMin, powerplant.PMax, payload.Fuels.KerosinePrice));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return powerplantsAvailable;
    }

    private PowerplantUsageDTO BuildPowerplantUsage(List<Powerplant> powerplants)
    {
        var powerplantUsage = new PowerplantUsageDTO
        {
            Powerplants = powerplants.Select(x => new PowerplantProductionDTO() { Name = x.Name, P = x.PActual }).ToList()
        };
        return powerplantUsage;
    }
}