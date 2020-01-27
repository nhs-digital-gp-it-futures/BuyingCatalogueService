namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands.BrowserBased
{
    public interface IUpdateBrowserBasedConnectivityAndResolutionData
    {
        string MinimumConnectionSpeed { get; }

        string MinimumDesktopResolution { get; }
    }
}
