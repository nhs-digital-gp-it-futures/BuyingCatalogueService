using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopHardwareRequirements
{
    internal sealed class UpdateNativeDesktopHardwareRequirementsHandler : Handler<UpdateNativeDesktopHardwareRequirementsCommand, ISimpleResult>
    {
        public UpdateNativeDesktopHardwareRequirementsHandler(
            IExecutor<UpdateNativeDesktopHardwareRequirementsCommand> executor,
            IValidator<UpdateNativeDesktopHardwareRequirementsCommand, ISimpleResult> validator)
            : base(executor, validator)
        {
        }
    }
}
