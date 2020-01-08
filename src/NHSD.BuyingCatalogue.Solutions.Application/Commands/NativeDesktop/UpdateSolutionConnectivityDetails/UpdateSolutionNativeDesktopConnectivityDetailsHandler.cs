using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionConnectivityDetails
{
    internal sealed class UpdateSolutionNativeDesktopConnectivityDetailsHandler : Handler<UpdateSolutionNativeDesktopConnectivityDetailsCommand, ISimpleResult>
    {
        public UpdateSolutionNativeDesktopConnectivityDetailsHandler(IExecutor<UpdateSolutionNativeDesktopConnectivityDetailsCommand> executor,
            IValidator<UpdateSolutionNativeDesktopConnectivityDetailsCommand, ISimpleResult> validator) : base(executor, validator)
        {
        }
    }
}
