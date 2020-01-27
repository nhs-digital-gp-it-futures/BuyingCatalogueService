using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class UpdateBrowserBasedConnectivityAndResolutionViewModel : IUpdateBrowserBasedConnectivityAndResolutionData
    {
        public string MinimumConnectionSpeed { get; set; }
        public string MinimumDesktopResolution { get; set; }
    }
}
