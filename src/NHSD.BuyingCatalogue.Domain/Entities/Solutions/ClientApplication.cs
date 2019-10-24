using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Domain.Entities.Solutions
{
    public class ClientApplication
    {
        public HashSet<string> ClientApplicationTypes { get; set; }

        public HashSet<string> BrowsersSupported { get; set; }

        public bool? MobileResponsive { get; set; }
    }
}
