using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Preview
{
    public class ClientApplicationTypesPreviewSubSections
    {
        [JsonProperty("browser-based")]
        public BrowserBasedPreviewSection BrowserBased { get; }

        [JsonIgnore]
        public bool HasData => BrowserBased != null;

        public ClientApplicationTypesPreviewSubSections(IClientApplication clientApplication)
        {
            BrowserBased = clientApplication?.ClientApplicationTypes?.Contains("browser-based") == true ?
                new BrowserBasedPreviewSection(clientApplication).IfPopulated() :
                null;
        }
    }
}
