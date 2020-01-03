using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionMobileThirdParty
{
    public class UpdateSolutionMobileThirdPartyCommand : IRequest<ISimpleResult>
    {
        public string Id { get; }

        public UpdateSolutionMobileThirdPartyViewModel ViewModel { get; }

        public UpdateSolutionMobileThirdPartyCommand(string id, UpdateSolutionMobileThirdPartyViewModel viewModel)
        {
            Id = id;
            ViewModel = viewModel;
        }
    }
}
