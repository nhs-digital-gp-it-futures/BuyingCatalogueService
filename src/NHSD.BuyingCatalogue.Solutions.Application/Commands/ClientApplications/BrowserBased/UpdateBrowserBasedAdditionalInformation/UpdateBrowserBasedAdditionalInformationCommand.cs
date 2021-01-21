using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateBrowserBasedAdditionalInformation
{
    public sealed class UpdateBrowserBasedAdditionalInformationCommand : IRequest<ISimpleResult>
    {
        public UpdateBrowserBasedAdditionalInformationCommand(string solutionId, string additionalInformation)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            AdditionalInformation = additionalInformation;
        }

        public string SolutionId { get; }

        public string AdditionalInformation { get; }
    }
}
