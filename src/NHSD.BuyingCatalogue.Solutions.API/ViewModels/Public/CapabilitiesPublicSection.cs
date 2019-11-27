using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Public
{
    public class CapabilitiesPublicSection
    {
        public CapabilitiesPublicSection(IEnumerable<string> capabilities)
            => Answers = new CapabilitiesMetPublicSection(capabilities);

        [JsonProperty("answers")]
        public CapabilitiesMetPublicSection Answers { get; }
    }
}
