using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.BrowserBased.UpdateSolutionBrowserHardwareRequirements
{
    public sealed class UpdateSolutionBrowserHardwareRequirementsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public string HardwareRequirements { get; }

        public UpdateSolutionBrowserHardwareRequirementsCommand(string solutionId, string hardwareRequirements)
        {
            SolutionId = solutionId.ThrowIfNull();
            HardwareRequirements = hardwareRequirements;
        }
    }
}
