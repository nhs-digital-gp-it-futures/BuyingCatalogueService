using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserHardwareRequirements
{
    public sealed class UpdateSolutionBrowserHardwareRequirementsCommand : IRequest<UpdateSolutionBrowserHardwareRequirementsValidationResult>
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
