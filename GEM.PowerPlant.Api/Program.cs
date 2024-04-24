using FluentValidation;
using FluentValidation.AspNetCore;
using GEM.PowerPlant.Application.Commands.Entities;
using GEM.PowerPlant.Application.Validators;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApi.Infrastructure;
using WebApi.ServiceCollection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.ConfigureApiBehaviorOptions(opts =>
	{
		//opts.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory.Build;
	})
	.AddJsonOptions(opts =>
	{
		opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

builder.Services.AddFluentValidationAutoValidation(fv => fv.DisableDataAnnotationsValidation = true)
	.AddFluentValidationClientsideAdapters()
	.AddValidatorsFromAssemblyContaining(typeof(FuelViewModelValidator));

builder.Services.AddExceptionHandler<GloabalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "Production Plan API",
		Description = "An ASP.NET Core Web API to plan power production",
	});
});

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<ProductionPlanCommand>());

builder.Services.AddApplication();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapControllers();

app.Run();

public partial class Program { }
