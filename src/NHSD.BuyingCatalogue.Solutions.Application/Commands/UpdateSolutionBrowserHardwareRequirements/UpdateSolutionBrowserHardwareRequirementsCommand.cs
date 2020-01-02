using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserHardwareRequirements
{
    public sealed class UpdateSolutionBrowserHardwareRequirementsCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public UpdateSolutionBrowserHardwareRequirementsViewModel UpdateSolutionHardwareRequirementsViewModel { get; }

        public UpdateSolutionBrowserHardwareRequirementsCommand(string solutionId, UpdateSolutionBrowserHardwareRequirementsViewModel updateSolutionHardwareRequirementsViewModel)
        {
            SolutionId = solutionId.ThrowIfNull();
            UpdateSolutionHardwareRequirementsViewModel = updateSolutionHardwareRequirementsViewModel.ThrowIfNull();
        }
    }
}
