using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionNativeMobileAdditionalInformation
{
    public sealed class UpdateSolutionNativeMobileAdditionalInformationCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string AdditionalInformation { get; }

        public UpdateSolutionNativeMobileAdditionalInformationCommand(string solutionId, string additionalInformation)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            AdditionalInformation = additionalInformation;
        }
    }
}
