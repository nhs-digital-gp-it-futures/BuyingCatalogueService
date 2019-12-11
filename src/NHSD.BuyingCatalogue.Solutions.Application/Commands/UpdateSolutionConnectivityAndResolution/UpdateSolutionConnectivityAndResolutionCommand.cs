using MediatR;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionConnectivityAndResolution
{
    public class UpdateSolutionConnectivityAndResolutionCommand : IRequest<UpdateSolutionConnectivityAndResolutionValidationResult>
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
