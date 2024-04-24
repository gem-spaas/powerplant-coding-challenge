using GEM.PowerPlant.Application.Commands.Entities;
using GEM.PowerPlant.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductionPlanController : ControllerBase
	{

		private readonly ILogger<ProductionPlanController> _logger;
		private readonly IMediator _mediator;

		public ProductionPlanController(IMediator mediator, ILogger<ProductionPlanController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		[HttpPost(Name = "ProductionPlan")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]				  
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public async Task<IResult> GenerateProductionPlan([FromBody] ProductionPlanViewModel productionPlan)
		{
			return Results.Ok(await _mediator.Send(new ProductionPlanCommand(productionPlan)));
		}
	}
}
