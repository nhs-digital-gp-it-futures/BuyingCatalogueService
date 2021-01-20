using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    public sealed class ClaimedCapabilityDto : IClaimedCapability
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public IEnumerable<IClaimedCapabilityEpic> ClaimedEpics { get; set; }
    }
}
