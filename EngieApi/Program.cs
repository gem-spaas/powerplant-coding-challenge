using EngieApi;
using EngieApi.Handlers;
using EngieApi.Processing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ProductionPlanHandler>();
builder.Services.AddScoped<ILoadPlanCalculator, LoadPlanCalculator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<ErrorLoggingMiddleware>();

app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.UseErrorLogging();
app.Run();
