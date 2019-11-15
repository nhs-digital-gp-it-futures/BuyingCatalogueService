using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.API.ViewModels.Public
{
    public class CapabilitiesPublicSection
    {
        [JsonProperty("answers")]
        public CapabilitiesMetPublicSection CapabilitiesMet { get; }
    }
}
