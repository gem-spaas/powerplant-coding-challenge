using Microsoft.AspNetCore.Mvc;
using PowerPlant.Api;
using PowerPlant.Models;
using PowerPlant.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPowerPlantService, PowerPlantService>();
builder.Services.AddScoped<IProductionPlanService, ProductionPlanService>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.MapPost("/productionplan", ([FromServices] IProductionPlanService productionPlanService, [FromBody] RequestedLoad requestedLoad)
    => productionPlanService.GetProductionPlan(requestedLoad));

app.Run();

