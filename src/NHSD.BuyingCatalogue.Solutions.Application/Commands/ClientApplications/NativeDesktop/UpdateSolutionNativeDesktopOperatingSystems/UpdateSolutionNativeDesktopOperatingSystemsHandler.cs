using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopOperatingSystems
{
    internal sealed class UpdateSolutionNativeDesktopOperatingSystemsHandler : Handler<UpdateSolutionNativeDesktopOperatingSystemsCommand, ISimpleResult>
    {
        public UpdateSolutionNativeDesktopOperatingSystemsHandler(IExecutor<UpdateSolutionNativeDesktopOperatingSystemsCommand> executor,
            IValidator<UpdateSolutionNativeDesktopOperatingSystemsCommand, ISimpleResult> validator) : base(executor, validator)
        {
        }
    }
}
