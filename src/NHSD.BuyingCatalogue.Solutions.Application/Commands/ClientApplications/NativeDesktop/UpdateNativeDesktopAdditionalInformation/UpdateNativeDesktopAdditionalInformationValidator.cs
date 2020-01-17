using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeDesktop.UpdateNativeDesktopAdditionalInformation
{
    internal sealed class UpdateNativeDesktopAdditionalInformationValidator : IValidator<UpdateNativeDesktopAdditionalInformationCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateNativeDesktopAdditionalInformationCommand command)
         => new MaxLengthValidator()
                .Validate(command.AdditionalInformation, 500, "additional-information")
                .Result();
    }
}
