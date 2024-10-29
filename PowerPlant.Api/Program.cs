using Microsoft.AspNetCore.Mvc.Infrastructure;
using PowerPlant.Api.Errors;
using PowerPlant.Application.Extensions;
using PowerPlant.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddSingleton<ProblemDetailsFactory, PowerPlantProblemDetailsFactory>();
builder.Services.AddDomainServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program { }