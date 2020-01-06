using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserHardwareRequirements
{
    public sealed class UpdateSolutionBrowserHardwareRequirementsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionBrowserHardwareRequirementsViewModel Data { get; }

        public UpdateSolutionBrowserHardwareRequirementsCommand(string solutionId, UpdateSolutionBrowserHardwareRequirementsViewModel data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
            Data.HardwareRequirements = Data.HardwareRequirements?.Trim();
        }
    }
}
