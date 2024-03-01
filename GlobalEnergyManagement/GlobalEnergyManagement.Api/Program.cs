using GlobalEnergyManagement.Api;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

var app = builder.Build();



app.Run();
