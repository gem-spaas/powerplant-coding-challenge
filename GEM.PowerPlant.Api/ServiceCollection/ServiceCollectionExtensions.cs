
using GEM.PowerPlant.Application.Commands.Contracts;
using GEM.PowerPlant.Application.Contracts;
using GEM.PowerPlant.Application.Services;

namespace WebApi.ServiceCollection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddScoped<IPowerplantService, PowerplantService>();
			services.AddSingleton<IPoweplantPlannerSelector, PoweplantPlannerSelector>();
			services.AddSingleton<IPoweplantPlanner, WindTurbinePowerplantPlanner>();
			services.AddSingleton<IPoweplantPlanner, GasfiredPowerplantPlanner>();
			services.AddSingleton<IPoweplantPlanner, TurbojetPowerplantPlanner>();

			return services;
		}
	}
}
