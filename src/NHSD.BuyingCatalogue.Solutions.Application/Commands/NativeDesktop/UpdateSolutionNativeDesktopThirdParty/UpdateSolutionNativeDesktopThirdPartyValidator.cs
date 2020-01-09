using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeDesktop.UpdateSolutionNativeDesktopThirdParty
{
    internal sealed class UpdateSolutionNativeDesktopThirdPartyValidator : IValidator<UpdateSolutionNativeDesktopThirdPartyCommand, ISimpleResult>
    {
        public ISimpleResult Validate(UpdateSolutionNativeDesktopThirdPartyCommand command)
            => new MaxLengthValidator()
                .Validate(command.Data.ThirdPartyComponents, 500, "third-party-components")
                .Validate(command.Data.DeviceCapabilities, 500, "device-capabilities")
                .Result();
    }
}
