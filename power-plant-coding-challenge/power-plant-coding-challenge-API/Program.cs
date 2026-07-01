using FluentValidation;
using MediatR;
using power_plant_coding_challenge_API.Middlewares;
using power_plant_coding_challenge_core.Features.CalculateProductionPlan;
using power_plant_coding_challenge_core.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ProductionPlanOptions>(builder.Configuration.GetSection("ProductionPlan"));

builder.Services.AddMediatR(cfg =>
{
    cfg.LicenseKey = builder.Configuration["MediatR:LicenseKey"];
    cfg.RegisterServicesFromAssemblies(typeof(CalculateProductionPlanHandler).Assembly);
});

builder.Services.AddValidatorsFromAssemblyContaining<CalculateProductionPlanCommandValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.UseMiddleware<CustomExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
