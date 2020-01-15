using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateSolutionNativeDesktopThirdParty
{
    internal sealed class UpdateSolutionNativeDesktopThirdPartyHandler : Handler<UpdateSolutionNativeDesktopThirdPartyCommand, ISimpleResult>
    {
        public UpdateSolutionNativeDesktopThirdPartyHandler(IExecutor<UpdateSolutionNativeDesktopThirdPartyCommand> executor,
            IValidator<UpdateSolutionNativeDesktopThirdPartyCommand, ISimpleResult> validator) : base(executor, validator)
        {
        }
    }
}
