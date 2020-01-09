namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands
{
    public interface IUpdateNativeDesktopThirdPartyData
    {
        string ThirdPartyComponents { get; }

        string DeviceCapabilities { get; }

        IUpdateNativeDesktopThirdPartyData Trim();
    }
}
