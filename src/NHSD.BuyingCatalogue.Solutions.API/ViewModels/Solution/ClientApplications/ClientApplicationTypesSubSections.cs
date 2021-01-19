using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeDesktop;
using NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications
{
    public sealed class ClientApplicationTypesSubSections
    {
        public ClientApplicationTypesSubSections(IClientApplication clientApplication)
        {
            BrowserBased = clientApplication?.ClientApplicationTypes?.Contains("browser-based") == true ?
                new BrowserBasedSection(clientApplication).IfPopulated() :
                null;

            NativeMobile = clientApplication?.ClientApplicationTypes?.Contains("native-mobile") == true
                ? new NativeMobileSection(clientApplication).IfPopulated()
                : null;

            NativeDesktop = clientApplication?.ClientApplicationTypes?.Contains("native-desktop") == true
                ? new NativeDesktopSection(clientApplication).IfPopulated()
                : null;
        }

        [JsonProperty("browser-based")]
        public BrowserBasedSection BrowserBased { get; }

        [JsonProperty("native-mobile")]
        public NativeMobileSection NativeMobile { get; }

        [JsonProperty("native-desktop")]
        public NativeDesktopSection NativeDesktop { get; }

        [JsonIgnore]
        public bool HasData => BrowserBased is not null || NativeMobile is not null || NativeDesktop is not null;
    }
}
