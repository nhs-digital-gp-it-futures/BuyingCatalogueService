using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopAdditionalInformation
{
    public sealed class UpdateNativeDesktopAdditionalInformationCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string AdditionalInformation { get; }

        public UpdateNativeDesktopAdditionalInformationCommand(string solutionId, string additionalInformation)
        {
            SolutionId = solutionId.ThrowIfNull();
            AdditionalInformation = additionalInformation;
        }
    }
}
