using Microsoft.Extensions.DependencyInjection;
using PowerPlant.Application.PowerPlants;
using PowerPlant.Application.ProductionPlanning;
using PowerPlant.Domain.Planning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPlant.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductionPlanService, ProductionPlanService>();
        services.AddSingleton<IPowerPlantFactory, PowerPlantFactory>();
        services.AddSingleton<IProductionPlanner, ProductionPlanner>();
        return services;
    }
}
