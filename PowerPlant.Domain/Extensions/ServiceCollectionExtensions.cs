using Microsoft.Extensions.DependencyInjection;
using PowerPlant.Domain.Interfaces;
using PowerPlant.Domain.Services;

namespace PowerPlant.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ICostComputationService, CostComputationService>();
        services.AddScoped<IUnitCommitmentStrategyService, UnitCommitmentStrategyService>();

        return services;
    }
}