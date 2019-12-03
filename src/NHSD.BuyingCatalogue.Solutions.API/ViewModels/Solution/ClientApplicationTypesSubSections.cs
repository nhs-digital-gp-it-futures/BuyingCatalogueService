using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class ClientApplicationTypesSubSections
    {
        [JsonProperty("browser-based")]
        public BrowserBasedSection BrowserBased { get; }

        [JsonIgnore]
        public bool HasData => BrowserBased != null;

        public ClientApplicationTypesSubSections(IClientApplication clientApplication)
        {
            BrowserBased = clientApplication?.ClientApplicationTypes?.Contains("browser-based") == true ?
                new BrowserBasedSection(clientApplication).IfPopulated() :
                null;
        }
    }
}
