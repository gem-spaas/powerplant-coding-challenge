using PowerPlant.Api.Middlewares;
using PowerPlant.Application;
using PowerPlant.Application.ProductionPlanning;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapPost("/productionplan", (ProductionPlanRequest request, IProductionPlanService productionPlanService)
    => Results.Ok(productionPlanService.CalculateProductionPlan(request)));

app.Run();

public record ProductionPlanItem(string Name, decimal P);