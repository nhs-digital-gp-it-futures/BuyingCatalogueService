using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionConnectivityAndResolution
{
    public class UpdateSolutionConnectivityAndResolutionCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }
        public UpdateSolutionConnectivityAndResolutionViewModel ViewModel { get; }

        public UpdateSolutionConnectivityAndResolutionCommand(string id, UpdateSolutionConnectivityAndResolutionViewModel viewModel)
        {
            Id = id;
            ViewModel = viewModel;
        }
    }
}
