using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;
using Powerplant.Api.Application.Abstractions;
using Powerplant.Api.Application.Services;
using Powerplant.Api.Mapping;
using Powerplant.Api.Middleware;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

builder.Services.AddProblemDetails();
builder.Services.AddScoped<IProductionPlanService, ProductionPlanService>();
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(ProductionPlanMappingProfile));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapGet("/swagger/v1/openapi.yaml", async (IWebHostEnvironment environment, CancellationToken cancellationToken) =>
{
    var contractPath = Path.Combine(environment.ContentRootPath, "Contracts", "openapi.yaml");

    if (!File.Exists(contractPath))
    {
        return Results.NotFound(new { message = "OpenAPI contract was not found." });
    }

    var yaml = await File.ReadAllTextAsync(contractPath, cancellationToken);
    return Results.Text(yaml, "application/yaml");
})
.WithName("OpenApiYaml")
.ExcludeFromDescription();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/swagger";
    settings.DocumentPath = "/swagger/v1/openapi.yaml";
});

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

public partial class Program;
