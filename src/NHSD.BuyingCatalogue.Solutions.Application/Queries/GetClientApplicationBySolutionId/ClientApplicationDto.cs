using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.NativeDesktop;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId
{
    internal sealed class ClientApplicationDto : IClientApplication
    {
        public HashSet<string> ClientApplicationTypes { get; set; } = new HashSet<string>();

        public HashSet<string> BrowsersSupported { get; set; } = new HashSet<string>();

        public bool? MobileResponsive { get; set; }

        public IPlugins Plugins { get; set; }

        public string HardwareRequirements { get; set; }

        public string NativeMobileHardwareRequirements { get; set; }

        public string NativeDesktopHardwareRequirements { get; set; }

        public string AdditionalInformation { get; set; }
        
        public string MinimumConnectionSpeed { get; set; }

        public string MinimumDesktopResolution { get; set; }

        public bool? MobileFirstDesign { get; set; }

        public bool? NativeMobileFirstDesign { get; set; }

        public IMobileOperatingSystems MobileOperatingSystems { get; set; }

        public IMobileConnectionDetails MobileConnectionDetails { get; set; }

        public IMobileMemoryAndStorage MobileMemoryAndStorage { get; set; }

        public IMobileThirdParty MobileThirdParty { get; set; }

        public string NativeMobileAdditionalInformation { get; set; }

        public string NativeDesktopOperatingSystemsDescription { get; set; }

        public string NativeDesktopMinimumConnectionSpeed { get; set; }

        public INativeDesktopThirdParty NativeDesktopThirdParty { get; set; }

        public INativeDesktopMemoryAndStorage NativeDesktopMemoryAndStorage { get; set; }

        public string NativeDesktopAdditionalInformation { get; set; }
    }
}
