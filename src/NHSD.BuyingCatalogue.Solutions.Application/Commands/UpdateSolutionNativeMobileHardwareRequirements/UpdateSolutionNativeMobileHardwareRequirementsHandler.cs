using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionNativeMobileHardwareRequirements
{
    internal sealed class UpdateSolutionNativeMobileHardwareRequirementsHandler : Handler<UpdateSolutionNativeMobileHardwareRequirementsCommand, ISimpleResult>
    {
        public UpdateSolutionNativeMobileHardwareRequirementsHandler(IExecutor<UpdateSolutionNativeMobileHardwareRequirementsCommand> executor,
            IValidator<UpdateSolutionNativeMobileHardwareRequirementsCommand, ISimpleResult> validator) : base(executor, validator)
        {
        }
    }
}
