using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateBrowserBasedAdditionalInformation
{
    public sealed class UpdateBrowserBasedAdditionalInformationCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string AdditionalInformation { get; }

        public UpdateBrowserBasedAdditionalInformationCommand(string solutionId, string additionalInformation)
        {
            SolutionId = solutionId.ThrowIfNull();
            AdditionalInformation = additionalInformation;
        }
    }
}
