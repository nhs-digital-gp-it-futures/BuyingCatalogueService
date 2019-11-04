using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Solutions.Domain
{
    public interface IClientApplication
    {
        HashSet<string> ClientApplicationTypes { get; }
        HashSet<string> BrowsersSupported { get; }
        bool? MobileResponsive { get; }
    }
}
