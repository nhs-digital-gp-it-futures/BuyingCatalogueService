using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation
{
    public sealed class UpdateSolutionNativeMobileAdditionalInformationCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionNativeMobileAdditionalInformationViewModel Data { get; }

        public UpdateSolutionNativeMobileAdditionalInformationCommand(string solutionId, UpdateSolutionNativeMobileAdditionalInformationViewModel data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}
