using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IClientApplication
    {
        HashSet<string> ClientApplicationTypes { get; }
        HashSet<string> BrowsersSupported { get; }
        bool? MobileResponsive { get; }
        IPlugins Plugins { get; }
        string HardwareRequirements { get; }
        string NativeMobileHardwareRequirements { get; }
        string NativeDesktopHardwareRequirements { get; }
        string MinimumConnectionSpeed { get; }
        string MinimumDesktopResolution { get; }
        string AdditionalInformation { get; }
        bool? MobileFirstDesign { get; }
        bool? NativeMobileFirstDesign { get; }
        IMobileOperatingSystems MobileOperatingSystems { get; }
        IMobileConnectionDetails MobileConnectionDetails { get; }
        IMobileMemoryAndStorage MobileMemoryAndStorage { get; }
        IMobileThirdParty MobileThirdParty { get; }
        string NativeMobileAdditionalInformation { get; }
    }
}
