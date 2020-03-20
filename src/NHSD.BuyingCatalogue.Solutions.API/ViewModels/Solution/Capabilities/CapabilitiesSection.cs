using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Capabilities
{
    public sealed class CapabilitiesSection
    {
        [JsonProperty("answers")]
        public CapabilitiesSectionAnswers Answers { get; }

        public CapabilitiesSection(IEnumerable<IClaimedCapability> capabilities)
            => Answers = new CapabilitiesSectionAnswers(capabilities);

        public CapabilitiesSection IfPopulated()
            => Answers.HasData() ? this : null;
    }
}
