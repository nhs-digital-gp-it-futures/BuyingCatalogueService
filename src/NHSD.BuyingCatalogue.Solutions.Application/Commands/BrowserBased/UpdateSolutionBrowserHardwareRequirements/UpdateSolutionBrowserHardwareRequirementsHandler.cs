using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.BrowserBased.UpdateSolutionBrowserHardwareRequirements
{
    internal sealed class UpdateSolutionBrowserHardwareRequirementsHandler : Handler<UpdateSolutionBrowserHardwareRequirementsCommand, ISimpleResult>
    {
        public UpdateSolutionBrowserHardwareRequirementsHandler(IExecutor<UpdateSolutionBrowserHardwareRequirementsCommand> updateSolutionBrowserHardwareRequirementsExecutor,
            IValidator<UpdateSolutionBrowserHardwareRequirementsCommand, ISimpleResult> updateSolutionBrowserHardwareRequirementsValidator) : base(updateSolutionBrowserHardwareRequirementsExecutor, updateSolutionBrowserHardwareRequirementsValidator)
        {
        }
    }
}
