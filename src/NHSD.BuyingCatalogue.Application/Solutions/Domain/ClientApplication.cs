using System.Collections.Generic;
using NHSD.BuyingCatalogue.Contracts;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Application.Solutions.Domain
{
    internal class ClientApplication
    {
        public HashSet<string> ClientApplicationTypes { get; set; } = new HashSet<string>();

        public HashSet<string> BrowsersSupported { get; set; } = new HashSet<string>();

        public bool? MobileResponsive { get; set; }

        public Plugins Plugins { get; set; }
    }
}
