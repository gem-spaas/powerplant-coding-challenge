namespace GlobalEnergyManagement.Application.Endpoints;

public static class PowerPlantEndpoints
{
    public static void MapPowerPlantEndpoints(this IEndpointRouteBuilder group)
    {
        group.MapPost("/", CalculateProductionPlan);

    }

    private static async Task<IResult> CalculateProductionPlan()
    {
        try
        {

            return Results.Ok();
        }
        catch
        {
            return Results.Problem("An error happened while calculating production plan.");
        }
    }
}