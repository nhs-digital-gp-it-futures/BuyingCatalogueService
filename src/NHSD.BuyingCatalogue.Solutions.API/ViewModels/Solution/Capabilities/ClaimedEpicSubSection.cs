using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Capabilities
{
    public sealed class ClaimedEpicSubSection
    {
        public IEnumerable<ClaimedEpicAnswer> Met { get; }

        [JsonProperty("not-met")]
        public IEnumerable<ClaimedEpicAnswer> NotMet { get; }

        public ClaimedEpicSubSection(IEnumerable<IClaimedCapabilityEpic> claimedCapabilityEpics)
        {
            var isMetEpicLookup = claimedCapabilityEpics.ToLookup(cce => cce.IsMet, cce => new ClaimedEpicAnswer(cce));
            Met = isMetEpicLookup[true];
            NotMet = isMetEpicLookup[false];
        }
    }
}
