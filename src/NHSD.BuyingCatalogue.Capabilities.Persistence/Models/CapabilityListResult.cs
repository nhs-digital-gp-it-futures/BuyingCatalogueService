using System.Diagnostics.CodeAnalysis;
using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Capabilities.Persistence.Models
{
    [SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by Dapper")]
    internal sealed class CapabilityListResult : ICapabilityListResult
    {
        public string CapabilityReference { get; set; }

        public string Version { get; set; }

        public string Name { get; set; }

        public bool IsFoundation { get; set; }
    }
}
