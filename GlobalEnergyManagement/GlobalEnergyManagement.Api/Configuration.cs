namespace GlobalEnergyManagement.Api;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") 
                          ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") 
                          ?? builder.Services.BuildServiceProvider()?.GetService<IWebHostEnvironment>()?.EnvironmentName
                          ?? string.Empty;

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .Build();
        
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
    }

}