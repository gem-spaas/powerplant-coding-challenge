using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PowerPlant.Api.Enums;
using PowerPlant.Api.IntegrationTests.Models;
using PowerPlant.Api.Models;

namespace PowerPlant.Api.IntegrationTests;

public class ProductionPlanControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductionPlanControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Post_ValidInput_ReturnsExpectedResult()
    {
        //Arrange
        var input = new ProductionPlanRequest
        {
            Load = 910,
            Fuels = new FuelsDTO()
            {
                GasPricePerMWh = 13.4M,
                KerosinePricePerMWh = 50.8M,
                Co2PricePerTon = 20M,
                WindPercentage = 60
            },
            PowerPlants = new List<PowerPlantDTO>
            {
                new()
                {
                    Name = "gasfiredbig1",
                    Type = PowerPlantTypeDTO.gasfired,
                    Efficiency = 0.53M,
                    Pmin = 100M,
                    Pmax = 460M
                },
                new()
                {
                    Name = "gasfiredbig2",
                    Type = PowerPlantTypeDTO.gasfired,
                    Efficiency = 0.53M,
                    Pmin = 100M,
                    Pmax = 460M
                },
                new()
                {
                    Name = "gasfiredsomewhatsmaller",
                    Type = PowerPlantTypeDTO.gasfired,
                    Efficiency = 0.37M,
                    Pmin = 40M,
                    Pmax = 210M
                },
                new()
                {
                    Name = "tj1",
                    Type = PowerPlantTypeDTO.turbojet,
                    Efficiency = 0.3M,
                    Pmin = 0M,
                    Pmax = 16M
                },
                new()
                {
                    Name = "windpark1",
                    Type = PowerPlantTypeDTO.windturbine,
                    Efficiency = 1M,
                    Pmin = 0M,
                    Pmax = 150M
                },
                new()
                {
                    Name = "windpark2",
                    Type = PowerPlantTypeDTO.windturbine,
                    Efficiency = 1M,
                    Pmin = 0M,
                    Pmax = 36M
                }
            }
        };

        var expectedOutput = new List<ProductionPlanResponseDTO>
        {
            new()
            {
                Name = "windpark1",
                P = 90.0M
            },
            new()
            {
                Name = "windpark2",
                P = 21.6M
            },
            new()
            {
                Name = "gasfiredbig1",
                P = 460.0M
            },
            new()
            {
                Name = "gasfiredbig2",
                P = 338.4M
            },
            new()
            {
                Name = "gasfiredsomewhatsmaller",
                P = 0.0M
            },
            new()
            {
                Name = "tj1",
                P = 0.0M
            },
        };

        var jsonString = JsonSerializer.Serialize(input);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        
        //Act
        var response = await _client.PostAsync("/productionplan", content);
        
        //Assert
        response.EnsureSuccessStatusCode();

        var resultJson = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ProductionPlanResponseDTO[]>(resultJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        result.Should().NotBeNull();
        result.Length.Should().Be(6);
        result.Should().BeEquivalentTo(expectedOutput);
    }
    
    [Fact]
    public async Task Post_InValidInput_ReturnsExpected_ValidationException()
    {
        //Arrange
        var input = new ProductionPlanRequest
        {
            Load = 910,
            Fuels = new FuelsDTO()
            {
                GasPricePerMWh = 13.4M,
                KerosinePricePerMWh = 50.8M,
                Co2PricePerTon = 20M,
                WindPercentage = 60
            },
            PowerPlants = new List<PowerPlantDTO>
            {
                new()
                {
                    Name = "",
                    Type = PowerPlantTypeDTO.gasfired,
                    Efficiency = 0.53M,
                    Pmin = 100M,
                    Pmax = 460M
                }
            }
        };

        var jsonString = JsonSerializer.Serialize(input);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        
        //Act
        var response = await _client.PostAsync("/productionplan", content);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var resultJson = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ValidationErrorResponse>(resultJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        result.Title.Should().Be("Validation failures occurred.");
        result.ErrorDetails.Count.Should().Be(1);   
        foreach (var error in result.ErrorDetails)
        {
            error.Key.Should().Be("PowerPlants[0].Name");
            error.Value.Should().BeEquivalentTo(new List<string> { "Power plant name is required." });
        }
    }
    
    [Fact]
    public async Task Post_InValidLoadInput_ReturnsExpected_BusinessException()
    {
        //Arrange
        var input = new ProductionPlanRequest
        {
            Load = 2000,
            Fuels = new FuelsDTO()
            {
                GasPricePerMWh = 13.4M,
                KerosinePricePerMWh = 50.8M,
                Co2PricePerTon = 20M,
                WindPercentage = 60
            },
            PowerPlants = new List<PowerPlantDTO>
            {
                new()
                {
                    Name = "test",
                    Type = PowerPlantTypeDTO.gasfired,
                    Efficiency = 0.53M,
                    Pmin = 100M,
                    Pmax = 460M
                }
            }
        };

        var jsonString = JsonSerializer.Serialize(input);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        
        //Act
        var response = await _client.PostAsync("/productionplan", content);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var resultJson = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<BusinessErrorResponse>(resultJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        result.Title.Should().Be("Business failure occured.");
        result.ErrorDetails.Should().Be("Powerplants cannot generated enough power for the load requested.");
    }
}