using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.NativeMobile
{
    public sealed class NativeMobileResult
    {
        public NativeMobileResult(IClientApplication clientApplication)
        {
            NativeMobileSections = new NativeMobileSections(clientApplication);
        }

        [JsonProperty("sections")]
        public NativeMobileSections NativeMobileSections { get; }
    }
}
