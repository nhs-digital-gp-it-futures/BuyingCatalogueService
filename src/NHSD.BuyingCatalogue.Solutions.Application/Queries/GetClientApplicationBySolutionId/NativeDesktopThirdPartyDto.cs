using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId
{
    public sealed class NativeDesktopThirdPartyDto : INativeDesktopThirdParty
    {
        public string ThirdPartyComponents { get; internal set; }

        public string DeviceCapabilities { get; internal set; }
    }
}
