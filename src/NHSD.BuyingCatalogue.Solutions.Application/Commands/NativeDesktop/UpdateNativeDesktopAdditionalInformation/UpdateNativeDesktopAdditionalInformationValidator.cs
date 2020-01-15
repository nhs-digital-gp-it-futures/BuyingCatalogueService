using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopAdditionalInformation
{
    internal sealed class UpdateNativeDesktopAdditionalInformationValidator : IValidator<UpdateNativeDesktopAdditionalInformationCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateNativeDesktopAdditionalInformationCommand updateNativeDesktopAdditionalInformationCommand)
         => new MaxLengthValidator()
                .Validate(updateNativeDesktopAdditionalInformationCommand.AdditionalInformation, 500, "additional-information")
                .Result();
    }
}
