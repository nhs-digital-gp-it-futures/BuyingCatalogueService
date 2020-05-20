﻿using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Epics
{
    internal sealed class UpdateClaimedEpicListRequest : IUpdateClaimedEpicListRequest
    {
        public IEnumerable<IClaimedEpicResult> ClaimedEpics { get; internal set; }

        public UpdateClaimedEpicListRequest(IEnumerable<IClaimedEpicResult> claimedEpics)
        {
            ClaimedEpics = claimedEpics ?? throw new ArgumentNullException(nameof(claimedEpics));
        }
    }
}
