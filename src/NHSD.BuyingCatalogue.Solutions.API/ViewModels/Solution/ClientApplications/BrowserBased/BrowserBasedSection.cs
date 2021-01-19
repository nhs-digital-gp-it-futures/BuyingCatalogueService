using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.ClientApplications.BrowserBased
{
    public sealed class BrowserBasedSection
    {
        public BrowserBasedSection(IClientApplication clientApplication)
        {
            Sections = new BrowserBasedSubSections(clientApplication);
        }

        [JsonProperty("sections")]
        public BrowserBasedSubSections Sections { get; }

        public BrowserBasedSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }
    }
}
