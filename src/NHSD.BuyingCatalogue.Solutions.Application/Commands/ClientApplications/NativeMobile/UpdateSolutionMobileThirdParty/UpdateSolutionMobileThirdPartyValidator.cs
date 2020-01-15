using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileThirdParty
{
    internal sealed class
        UpdateSolutionMobileThirdPartyValidator : IValidator<UpdateSolutionMobileThirdPartyCommand, ISimpleResult>
    {
        private const int ComponentLength = 500;
        private const int CapabilityLength = 500;

        public ISimpleResult Validate(UpdateSolutionMobileThirdPartyCommand command)
            => new MaxLengthValidator()
                .Validate(command.Data.ThirdPartyComponents, ComponentLength, "third-party-components")
                .Validate(command.Data.DeviceCapabilities, CapabilityLength, "device-capabilities")
                .Result();
    }
}
