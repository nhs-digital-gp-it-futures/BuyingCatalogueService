using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileThirdParty
{
    internal sealed class
        UpdateSolutionMobileThirdPartyValidator : IValidator<UpdateSolutionMobileThirdPartyCommand, ISimpleResult>
    {
        private const int ComponentLength = 500;
        private const int CapabilityLength = 500;

        public ISimpleResult Validate(UpdateSolutionMobileThirdPartyCommand command)
            => new MaxLengthValidator()
                .Validate(command.ViewModel.ThirdPartyComponents, ComponentLength, "third-party-components")
                .Validate(command.ViewModel.DeviceCapabilities, CapabilityLength, "device-capabilities")
                .Result();
    }
}
