using System;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests.Models
{
    internal sealed class EpicDetails
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Guid CapabilityId { get; set; }

        public string SourceUrl { get; set; }

        public int CompliancyLevelId { get; set; }

        public bool Active { get; set; }
    }
}
