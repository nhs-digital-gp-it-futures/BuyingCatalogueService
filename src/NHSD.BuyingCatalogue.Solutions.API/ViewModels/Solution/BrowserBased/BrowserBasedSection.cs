using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class BrowserBasedSection
    {
        [JsonProperty("sections")]
        public BrowserBasedSubSections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedSection"/> class.
        /// </summary>
        public BrowserBasedSection(IClientApplication clientApplication)
        {
            Sections = new BrowserBasedSubSections(clientApplication);
        }

        public BrowserBasedSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }
    }
}
