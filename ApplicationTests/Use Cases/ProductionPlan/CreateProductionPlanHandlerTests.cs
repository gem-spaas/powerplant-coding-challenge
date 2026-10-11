
using Application.Use_Cases.ProductionPlan;
using Domain.Services;

namespace Application.Tests.Use_Cases.ProductionPlan;

public sealed class ProductionPlanHandlerTests
{
    private readonly ProductionPlanHandler _handler =
        new(new ProductionPlanCalculator());

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenLoadIsZero()
    {
        // Arrange
        var command = CreateCommand(load: 0);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
            
        Assert.All(
            result.Value!,
            production => Assert.Equal(0, production.P));
    }

    [Fact]
    public async Task Handle_ShouldReturnProductionPlan_WhenLoadCanBeSatisfied()
    {
        // Arrange
        var command = CreateCommand(load: 80);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        var productions = result.Value!;

        Assert.Equal(2, productions.Count);

        Assert.Equal(
            80,
            productions.Sum(p => p.P),
            precision: 6);

        Assert.Contains(
            productions,
            p => p.Name == "wind1" && p.P == 50);

        Assert.Contains(
            productions,
            p => p.Name == "gas1" && p.P == 30);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenLoadCannotBeSatisfied()
    {
        // Arrange
        var command = CreateCommand(load: 250);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("InsufficientCapacity", result.Error.Code);
    }

    private static CreateProductionPlanCommand CreateCommand(double load)
    {
        return new CreateProductionPlanCommand(
            load,
            [
                new PowerPlantDto("wind1", "WindTurbine", 0.5, 0, 100),
                new PowerPlantDto("gas1", "GasFired", 0.5, 0, 100)
            ],
            new FuelDto(50,100,0,50));
    }
}
