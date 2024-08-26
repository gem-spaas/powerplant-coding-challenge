using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using PowerPlantCodingChallenge.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductionPlanService, ProductionPlanService>();
builder.Services.AddControllers(options =>
{
    options.ModelMetadataDetailsProviders.Add(new NewtonsoftJsonValidationMetadataProvider());
}).AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
