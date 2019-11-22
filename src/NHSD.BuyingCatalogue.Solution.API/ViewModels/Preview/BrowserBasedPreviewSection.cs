using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Solutions;

namespace NHSD.BuyingCatalogue.Solution.API.ViewModels.Preview
{
    public class BrowserBasedPreviewSection
    {
        [JsonProperty("sections")]
        public BrowserBasedPreviewSubSections Sections { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BrowserBasedPreviewSection"/> class.
        /// </summary>
        public BrowserBasedPreviewSection(IClientApplication clientApplication)
        {
            Sections = new BrowserBasedPreviewSubSections(clientApplication);
        }

        public BrowserBasedPreviewSection IfPopulated()
        {
            return Sections.HasData ? this : null;
        }
    }
}
