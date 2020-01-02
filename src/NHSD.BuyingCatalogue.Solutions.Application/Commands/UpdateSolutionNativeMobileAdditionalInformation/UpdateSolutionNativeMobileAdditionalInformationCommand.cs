using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserAdditionalInformation;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileAdditionalInformation
{
    public sealed class UpdateSolutionNativeMobileAdditionalInformationCommand : IRequest<MaxLengthResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionNativeMobileAdditionalInformationViewModel UpdateSolutionNativeMobileAdditionalInformationViewModel { get; }
        public UpdateSolutionBrowserAdditionalInformationViewModel UpdateSolutionBrowserAdditionalInformationViewModel { get; set; }

        public UpdateSolutionNativeMobileAdditionalInformationCommand(string solutionId, UpdateSolutionNativeMobileAdditionalInformationViewModel updateSolutionNativeMobileAdditionalInformationViewModel)
        {
            SolutionId = solutionId.ThrowIfNull();
            UpdateSolutionNativeMobileAdditionalInformationViewModel = updateSolutionNativeMobileAdditionalInformationViewModel.ThrowIfNull();
        }
    }
}
