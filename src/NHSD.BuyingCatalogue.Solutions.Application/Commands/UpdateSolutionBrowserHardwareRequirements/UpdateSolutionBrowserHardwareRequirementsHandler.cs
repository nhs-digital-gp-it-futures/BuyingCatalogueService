using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionBrowserHardwareRequirements
{
    internal sealed class UpdateSolutionBrowserHardwareRequirementsHandler : Handler<UpdateSolutionBrowserHardwareRequirementsCommand, MaxLengthResult>
    {
        public UpdateSolutionBrowserHardwareRequirementsHandler(IExecutor<UpdateSolutionBrowserHardwareRequirementsCommand> updateSolutionBrowserHardwareRequirementsExecutor,
            IValidator<UpdateSolutionBrowserHardwareRequirementsCommand, MaxLengthResult> updateSolutionBrowserHardwareRequirementsValidator) : base(updateSolutionBrowserHardwareRequirementsExecutor, updateSolutionBrowserHardwareRequirementsValidator)
        {
        }
    }
}
