﻿using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IClaimedCapability
    {
        string Name { get; }

        string Version { get; }

        string Description { get; }

        string Link { get; }

        IEnumerable<IClaimedCapabilityEpic> ClaimedEpics { get; }
    }
}
