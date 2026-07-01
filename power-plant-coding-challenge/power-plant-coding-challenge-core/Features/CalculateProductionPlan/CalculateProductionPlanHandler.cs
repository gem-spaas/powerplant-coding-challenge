using MediatR;
using Microsoft.Extensions.Options;
using power_plant_coding_challenge_core.Options;
using power_plant_coding_challenge_domain.Enums;
using power_plant_coding_challenge_domain.Models;

namespace power_plant_coding_challenge_core.Features.CalculateProductionPlan;

public class CalculateProductionPlanHandler(IOptions<ProductionPlanOptions> options) : 
    IRequestHandler<CalculateProductionPlanCommand, CalculateProductionPlanResponse>
{
    public async Task<CalculateProductionPlanResponse> Handle(CalculateProductionPlanCommand request, CancellationToken cancellationToken)
    {
        var includeCo2Costs = options.Value.IncludeCo2Costs;
        List<CalculateProductionPlanResult>? resultWithWindPowerplants = null;

        var orderedPowerplants = request.Powerplants.MeritOrder(request.Fuels, includeCo2Costs);

        if (request.Load == 0) return new CalculateProductionPlanResponse(orderedPowerplants.CreateResultWith0P());

        List<Powerplant> windOnlyPowerplants, nonWindPowerplants;
        orderedPowerplants.SplitByPowerplantType(PowerplantType.Windturbine, out windOnlyPowerplants, out nonWindPowerplants);

        //Calculate the total wind power available to check if it can meet the load.
        var totalWindPower = windOnlyPowerplants.Sum(powerplant => powerplant.GetActualPMax(request.Fuels));
        var loadAfterWindPower = request.Load - totalWindPower;

        //the wind power can meet the load exactly, we use it and set the non-wind powerplants to 0.
        if (loadAfterWindPower == 0) resultWithWindPowerplants = new List<CalculateProductionPlanResult>();
        //the wind power can meet part of the load, we try to find a combination of non-wind powerplants that can meet the remaining load.
        else if (loadAfterWindPower > 0)
            resultWithWindPowerplants = nonWindPowerplants.GetUsablePowerplantsFromList(request.Fuels, loadAfterWindPower);
        //Whatever were the previous result with wind powerplants, we also need to calculate the result without wind powerplants to compare the cost and see if we can meet the load with non-wind powerplants only.
        var resultWithoutWindPowerplants = nonWindPowerplants.GetUsablePowerplantsFromList(request.Fuels, request.Load);

        var result = resultWithWindPowerplants.CompareResultCostWith(resultWithoutWindPowerplants, nonWindPowerplants, request.Fuels, includeCo2Costs);

        var useWind = result == resultWithWindPowerplants;

        return new CalculateProductionPlanResponse(result.OrderResultsByMeritOrder(orderedPowerplants, useWind, request.Fuels));
    }
}
