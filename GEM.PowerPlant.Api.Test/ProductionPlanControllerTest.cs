using FluentAssertions;
using GEM.PowerPlant.Api.Test.Extensions;
using GEM.PowerPlant.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace GEM.PowerPlant.Api.Test
{
	public class ProductionPlanControllerTest : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;

		private static JsonSerializerOptions DefaultJsonSerializerOptions()
		{
			var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
			options.Converters.Add(new JsonStringEnumConverter());
			return options;
		}


		public ProductionPlanControllerTest(WebApplicationFactory<Program> factory)
		{
			_factory = factory;
		}

		[Fact]
		public async Task Should_Return_Production_Powerplants_plan()
		{

			var req = JsonSerializer.Deserialize<ProductionPlanViewModel>("payload3.json".Load(), DefaultJsonSerializerOptions());

			var client = _factory.CreateClient();

			var response = await client.PostAsJsonAsync("/productionPlan", req, DefaultJsonSerializerOptions(), CancellationToken.None);

			((short)response.StatusCode).Should().Be(StatusCodes.Status200OK);

			var result = await response.Content.ReadFromJsonAsync<List<PowerplantResponseViewModel>>();

			result.Should().NotBeNull();
			result.Count.Should().Be(req.Powerplants.Count);

			result.Should().SatisfyRespectively(item =>
			{
				item.Name.Should().Be("windpark1");
				item.DeliveredPower.Should().Be(90);
			},
			item =>
			{
				item.Name.Should().Be("windpark2");
				item.DeliveredPower.Should().Be(21.6);
			},
			item =>
			{
				item.Name.Should().Be("gasfiredbig1");
				item.DeliveredPower.Should().Be(460);
			},
			item =>
			{
				item.Name.Should().Be("gasfiredbig2");
				item.DeliveredPower.Should().Be(338.4);
			},
			item =>
			{
				item.Name.Should().Be("gasfiredsomewhatsmaller");
				item.DeliveredPower.Should().Be(0);
			},
			item =>
			{
				item.Name.Should().Be("tj1");
				item.DeliveredPower.Should().Be(0);
			});

		}

		[Fact]
		public async Task Should_Return_Bad_Request_When_Requested_Load_Is_Too_High()
		{

			var req = JsonSerializer.Deserialize<ProductionPlanViewModel>("load-too-high.json".Load(), DefaultJsonSerializerOptions());

			var client = _factory.CreateClient();

			var response = await client.PostAsJsonAsync("/productionPlan", req, DefaultJsonSerializerOptions(), CancellationToken.None);

			((short)response.StatusCode).Should().Be(StatusCodes.Status400BadRequest);

			var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();

			result.Detail.Should().NotBeEmpty();

		}

		[Fact]
		public async Task Should_Return_Bad_Request_When_Payload_Is_Invalid()
		{

			var req = JsonSerializer.Deserialize<ProductionPlanViewModel>("invalid-payload.json".Load(), DefaultJsonSerializerOptions());

			var client = _factory.CreateClient();

			var response = await client.PostAsJsonAsync("/productionPlan", req, DefaultJsonSerializerOptions(), CancellationToken.None);

			((short)response.StatusCode).Should().Be(StatusCodes.Status400BadRequest);
		}
	}
}
