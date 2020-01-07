using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateNativeDesktopHardwareRequirements
{
    internal sealed class UpdateNativeDesktopHardwareRequirementsValidator : IValidator<UpdateNativeDesktopHardwareRequirementsCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateNativeDesktopHardwareRequirementsCommand command)
            => new MaxLengthValidator()
                .Validate(command.HardwareRequirements, 500, "hardware-requirements")
                .Result();
    }
}
