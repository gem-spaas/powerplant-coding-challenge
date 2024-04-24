using GEM.PowerPlant.Application.ViewModels;
using MediatR;

namespace GEM.PowerPlant.Application.Commands.Entities
{
	public class ProductionPlanCommand : IRequest<List<PowerplantResponseViewModel>>
	{
		public ProductionPlanViewModel viewModel { get; }

		public ProductionPlanCommand(ProductionPlanViewModel viewModel)
		{
			this.viewModel = viewModel;
		}
	}
}
