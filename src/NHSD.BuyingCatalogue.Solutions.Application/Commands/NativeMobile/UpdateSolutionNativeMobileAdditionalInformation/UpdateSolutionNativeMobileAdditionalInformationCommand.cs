using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation
{
    public sealed class UpdateSolutionNativeMobileAdditionalInformationCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionNativeMobileAdditionalInformationViewModel UpdateSolutionNativeMobileAdditionalInformationViewModel { get; }

        public UpdateSolutionNativeMobileAdditionalInformationCommand(string solutionId, UpdateSolutionNativeMobileAdditionalInformationViewModel updateSolutionNativeMobileAdditionalInformationViewModel)
        {
            SolutionId = solutionId.ThrowIfNull();
            UpdateSolutionNativeMobileAdditionalInformationViewModel = updateSolutionNativeMobileAdditionalInformationViewModel.ThrowIfNull();
        }
    }
}
