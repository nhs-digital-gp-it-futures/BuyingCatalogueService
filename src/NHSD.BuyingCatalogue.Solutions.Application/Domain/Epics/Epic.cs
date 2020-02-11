using System;
using System.Collections.Generic;
using System.Text;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Epics
{
    internal sealed class Epic
    {
        public IEnumerable<ClaimedEpic> Epics { get; set; }
    }
}
