using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeDesktop
{
    public sealed class NativeDesktopResult
    {
        public NativeDesktopResult(IClientApplication clientApplication)
        {
            NativeDesktopSections = new NativeDesktopSections(clientApplication);
        }

        [JsonProperty("sections")]
        public NativeDesktopSections NativeDesktopSections { get; }
    }
}
