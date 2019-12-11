using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation
{
    public sealed class UpdateSolutionBrowserAdditionalInformationCommand : IRequest<UpdateSolutionBrowserAdditionalInformationValidationResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionBrowserAdditionalInformationViewModel UpdateSolutionBrowserAdditionalInformationViewModel { get; }

        public UpdateSolutionBrowserAdditionalInformationCommand(string solutionId, UpdateSolutionBrowserAdditionalInformationViewModel updateSolutionBrowserAdditionalInformationViewModel)
        {
            SolutionId = solutionId.ThrowIfNull();
            UpdateSolutionBrowserAdditionalInformationViewModel = updateSolutionBrowserAdditionalInformationViewModel.ThrowIfNull();
        }
    }
}
