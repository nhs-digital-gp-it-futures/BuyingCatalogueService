using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal class ClientApplication
    {
        public HashSet<string> ClientApplicationTypes { get; set; } = new HashSet<string>();

        public HashSet<string> BrowsersSupported { get; set; } = new HashSet<string>();

        public bool? MobileResponsive { get; set; }

        public Plugins Plugins { get; set; }
    }
}
