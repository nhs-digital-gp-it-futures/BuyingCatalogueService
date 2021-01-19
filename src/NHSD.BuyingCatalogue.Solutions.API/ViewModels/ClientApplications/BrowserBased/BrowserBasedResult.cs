using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.ClientApplications.BrowserBased
{
    public sealed class BrowserBasedResult
    {
        public BrowserBasedResult(IClientApplication clientApplication)
        {
            BrowserBasedDashboardSections = new BrowserBasedDashboardSections(clientApplication);
        }

        [JsonProperty("sections")]
        public BrowserBasedDashboardSections BrowserBasedDashboardSections { get; }
    }
}
