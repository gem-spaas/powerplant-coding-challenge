using GlobalEnergyManagement.Application.Contracts;
using GlobalEnergyManagement.Application.DTOs;

namespace GlobalEnergyManagement.Application.Endpoints;

public static class PowerPlantEndpoints
{
    public static void MapPowerPlantEndpoints(this IEndpointRouteBuilder group)
    {
        group.MapPost("/", CalculateProductionPlan);

    }

    private static async Task<IResult> CalculateProductionPlan(IPowerPlantService powerPlantService, PowerPlantPayload payload)
    {
        try
        {
            var result = await powerPlantService.CalculateProductionPlan(payload);
            
            return Results.Ok(result);
        }
        catch
        {
            return Results.Problem("An error happened while calculating production plan.");
        }
    }
}