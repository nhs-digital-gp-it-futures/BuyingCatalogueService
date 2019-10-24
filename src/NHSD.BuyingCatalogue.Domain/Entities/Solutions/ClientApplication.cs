using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Domain.Entities.Solutions
{
    public class ClientApplication
    {
        public HashSet<string> ClientApplicationTypes { get; set; } = new HashSet<string>();

        public HashSet<string> BrowsersSupported { get; set; } = new HashSet<string>();

        public bool? MobileResponsive { get; set; }
    }
}
