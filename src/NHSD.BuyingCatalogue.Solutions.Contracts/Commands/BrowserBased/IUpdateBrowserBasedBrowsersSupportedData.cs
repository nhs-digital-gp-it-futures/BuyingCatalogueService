using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased
{
    public interface IUpdateBrowserBasedBrowsersSupportedData
    {
        HashSet<string> BrowsersSupported { get; }

        string MobileResponsive { get; }
    }
}
