using Carter;
using Application;
using ProductionPlanHost.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddOpenApi();

builder.Services.AddCarter();

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<ExceptionHandlers>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapCarter();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();
