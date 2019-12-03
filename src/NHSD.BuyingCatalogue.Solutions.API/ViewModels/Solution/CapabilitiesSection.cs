using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution
{
    public class CapabilitiesSection
    {
        public CapabilitiesSection(IEnumerable<string> capabilities)
            => Answers = new CapabilitiesMetSection(capabilities);

        [JsonProperty("answers")]
        public CapabilitiesMetSection Answers { get; }
    }
}
