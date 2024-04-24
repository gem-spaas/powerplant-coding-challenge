using Application.Mappers;
using GEM.PowerPlant.Application.Commands.Contracts;
using GEM.PowerPlant.Application.Commands.Entities;
using GEM.PowerPlant.Application.ViewModels;
using Mapster;
using MediatR;

namespace GEM.PowerPlant.Application.Commands.Handlers
{
	public class ProductionPlanCommandHandler : IRequestHandler<ProductionPlanCommand, List<PowerplantResponseViewModel>>
	{
		private readonly IPowerplantService _powerplantService;

		public ProductionPlanCommandHandler(IPowerplantService powerplantService)
		{
			_powerplantService = powerplantService;
		}

		public Task<List<PowerplantResponseViewModel>> Handle(ProductionPlanCommand request, CancellationToken cancellationToken)
		{
			var vm = request.viewModel;

			var serviceResult = _powerplantService.GeneratePlan(vm.Powerplants.MapToDomain(), vm.Fuels.MapToDomain(), vm.Load);

			return Task.FromResult(serviceResult.Adapt<List<PowerplantResponseViewModel>>());
		}
	}
}
