using NHSD.BuyingCatalogue.Solutions.Application.Commands.Execution;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopAdditionalInformation
{
    internal sealed class UpdateNativeDesktopAdditionalInformationHandler : Handler<UpdateNativeDesktopAdditionalInformationCommand, ISimpleResult>
    {
        public UpdateNativeDesktopAdditionalInformationHandler(IExecutor<UpdateNativeDesktopAdditionalInformationCommand> updateNativeDesktopAdditionalInformationExecutor,
            IValidator<UpdateNativeDesktopAdditionalInformationCommand, ISimpleResult> updateNativeDesktopAdditionalInformationValidator) : base(updateNativeDesktopAdditionalInformationExecutor, updateNativeDesktopAdditionalInformationValidator)
        {
        }
    }
}
