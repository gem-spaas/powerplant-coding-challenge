using GlobalEnergyManagement.Application.Contracts;
using GlobalEnergyManagement.Application.Endpoints;
using GlobalEnergyManagement.Application.Services;

namespace GlobalEnergyManagement.Api;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPowerPlantService, PowerPlantService>();
        
        builder.Services.AddLogging();
        builder.Services.AddHttpLogging(options => {});
        
        builder.Services.AddHealthChecks();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    public static void RegisterMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHealthChecks("/health");
        
        app.UseHttpLogging();

        app.UseHttpsRedirection();
        
        app.MapGroup("/productionplan").MapPowerPlantEndpoints();
    }
}