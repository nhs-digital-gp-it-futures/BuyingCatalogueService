using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId
{
    public sealed class MobileThirdPartyDto : IMobileThirdParty
    {
        public string ThirdPartyComponents { get; internal set; }

        public string DeviceCapabilities { get; internal set; }
    }
}
