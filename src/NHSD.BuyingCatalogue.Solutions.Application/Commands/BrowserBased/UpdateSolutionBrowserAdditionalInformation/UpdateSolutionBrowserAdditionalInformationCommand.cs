using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserAdditionalInformation
{
    public sealed class UpdateSolutionBrowserAdditionalInformationCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionBrowserAdditionalInformationViewModel Data { get; }

        public UpdateSolutionBrowserAdditionalInformationCommand(string solutionId, UpdateSolutionBrowserAdditionalInformationViewModel data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
            Data.AdditionalInformation = Data.AdditionalInformation?.Trim();
        }
    }
}
