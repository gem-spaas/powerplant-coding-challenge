using Microsoft.AspNetCore.Mvc.Infrastructure;
using PowerPlantCC.Api.Common.Errors;
using PowerPlantCC.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

builder.Services.AddSingleton<ProblemDetailsFactory, PowerPlantProblemDetailsFactory>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");
app.MapControllers();
app.UseHttpsRedirection();
app.Run();