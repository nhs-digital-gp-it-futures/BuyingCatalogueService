using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.BrowserBased
{
    internal static class UpdateBrowserBasedViewModelExtensions
    {
        public static IUpdateBrowserBasedBrowsersSupportedData Trim(this IUpdateBrowserBasedBrowsersSupportedData data)
        {
            return new UpdateBrowserBasedBrowsersSupportedViewModel()
            {
                BrowsersSupported =
                    data.BrowsersSupported == null
                        ? new HashSet<string>()
                        : new HashSet<string>(data.BrowsersSupported.Where(x => !string.IsNullOrWhiteSpace(x))
                            .Select(x => x.Trim())),
                MobileResponsive = data.MobileResponsive?.Trim()
            };
        }
    }
}
