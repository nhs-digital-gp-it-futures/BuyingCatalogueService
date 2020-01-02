using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal class ClientApplication
    {
        public HashSet<string> ClientApplicationTypes { get; set; } = new HashSet<string>();

        public HashSet<string> BrowsersSupported { get; set; } = new HashSet<string>();

        public bool? MobileResponsive { get; set; }

        public Plugins Plugins { get; set; }

        public string MinimumConnectionSpeed { get; set; }

        public string MinimumDesktopResolution { get; set; }

        public string HardwareRequirements { get; set; }

        public string NativeMobileHardwareRequirements { get; set; }

        public string AdditionalInformation { get; set; }

        public bool? MobileFirstDesign { get; set; }

        public bool? NativeMobileFirstDesign { get; set; }

        public MobileOperatingSystems MobileOperatingSystems { get; set; }

        public MobileConnectionDetails MobileConnectionDetails { get; set; }

        public MobileMemoryAndStorage MobileMemoryAndStorage { get; set; }

        public MobileThirdParty MobileThirdParty { get; set; }

        public string NativeMobileAdditionalInformation { get; set; }
    }
}
