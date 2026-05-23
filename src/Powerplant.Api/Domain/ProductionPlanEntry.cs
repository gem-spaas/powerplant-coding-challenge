namespace Powerplant.Api.Domain
{
    public class ProductionPlanEntry
    {
        public required string Name { get; init; }

        public required decimal Power { get; init; }
    }
}
