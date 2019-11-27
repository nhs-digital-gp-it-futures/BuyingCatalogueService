using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class BrowserBasedPublicSection
    {
        [JsonProperty("sections")]
        public BrowserBasedPublicSubSections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedPublicSection"/> class.
        /// </summary>
        public BrowserBasedPublicSection(IClientApplication clientApplication)
        {
            Sections = new BrowserBasedPublicSubSections(clientApplication);
        }

        public BrowserBasedPublicSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }
    }
}
