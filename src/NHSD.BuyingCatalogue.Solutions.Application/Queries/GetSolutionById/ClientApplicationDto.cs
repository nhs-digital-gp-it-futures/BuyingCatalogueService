using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    internal class ClientApplicationDto : IClientApplication
    {
        public HashSet<string> ClientApplicationTypes { get; set; } = new HashSet<string>();

        public HashSet<string> BrowsersSupported { get; set; } = new HashSet<string>();

        public bool? MobileResponsive { get; set; }

        public IPlugins Plugins { get; set; }

        public string HardwareRequirements { get; set; }

        public string AdditionalInformation { get; set; }
        
        public string MinimumConnectionSpeed { get; set; }

        public string MinimumDesktopResolution { get; set; }

        public bool? MobileFirstDesign { get; set; }

        public IMobileOperatingSystems MobileOperatingSystems { get; set; }
    }
}
