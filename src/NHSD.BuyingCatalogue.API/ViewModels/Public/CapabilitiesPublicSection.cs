using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class CapabilitiesPublicSection
    {
        public CapabilitiesPublicSection()
        {
            CapabilitiesMet = new CapabilitiesMetPublicSection();
        }
        [JsonProperty("answers")]
        public CapabilitiesMetPublicSection CapabilitiesMet { get; }
    }
}
