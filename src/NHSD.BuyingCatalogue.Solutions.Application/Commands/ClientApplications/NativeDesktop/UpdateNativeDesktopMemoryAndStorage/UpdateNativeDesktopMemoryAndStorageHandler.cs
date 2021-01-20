using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopMemoryAndStorage
{
    internal sealed class UpdateNativeDesktopMemoryAndStorageHandler : Handler<UpdateNativeDesktopMemoryAndStorageCommand, ISimpleResult>
    {
        public UpdateNativeDesktopMemoryAndStorageHandler(
            IExecutor<UpdateNativeDesktopMemoryAndStorageCommand> executor,
            IValidator<UpdateNativeDesktopMemoryAndStorageCommand, ISimpleResult> validator)
            : base(executor, validator)
        {
        }
    }
}
