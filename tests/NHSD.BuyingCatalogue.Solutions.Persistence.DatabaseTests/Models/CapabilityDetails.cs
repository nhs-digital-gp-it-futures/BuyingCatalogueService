using System;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests.Models
{
    internal sealed class CapabilityDetails
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public string Reference { get; set; }

        public string Version { get; set; }

        public string SourceUrl { get; set; }
    }
}
