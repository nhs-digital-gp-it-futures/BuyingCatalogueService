using System;
using System.Collections.Generic;
using System.Linq;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Capabilities
{
    public sealed class ClaimedCapabilityEpicSection
    {
        public ClaimedCapabilityEpicSection(IEnumerable<IClaimedCapabilityEpic> claimedCapabilityEpics)
        {
            var mustEpics = claimedCapabilityEpics
                .Where(e => e.EpicCompliancyLevel.Equals("MUST", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var mayEpics = claimedCapabilityEpics
                .Where(e => e.EpicCompliancyLevel.Equals("MAY", StringComparison.OrdinalIgnoreCase))
                .ToList();

            Must = mustEpics.Any() ? new ClaimedEpicSubSection(mustEpics) : null;
            May = mayEpics.Any() ? new ClaimedEpicSubSection(mayEpics) : null;
        }

        public ClaimedEpicSubSection May { get; }

        public ClaimedEpicSubSection Must { get; }
    }
}
