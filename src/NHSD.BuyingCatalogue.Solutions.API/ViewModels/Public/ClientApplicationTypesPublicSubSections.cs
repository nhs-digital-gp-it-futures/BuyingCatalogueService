using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class ClientApplicationTypesPublicSubSections
    {
        [JsonProperty("browser-based")]
        public BrowserBasedPublicSection BrowserBased { get; }

        [JsonIgnore]
        public bool HasData => BrowserBased != null;

        public ClientApplicationTypesPublicSubSections(IClientApplication clientApplication)
        {
            BrowserBased = clientApplication?.ClientApplicationTypes?.Contains("browser-based") == true ?
                new BrowserBasedPublicSection(clientApplication).IfPopulated() :
                null;
        }
    }
}
