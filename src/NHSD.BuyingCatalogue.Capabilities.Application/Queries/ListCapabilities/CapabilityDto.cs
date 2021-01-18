using NHSD.BuyingCatalogue.Capabilities.Application.Domain;
using NHSD.BuyingCatalogue.Capabilities.Contracts;

namespace NHSD.BuyingCatalogue.Capabilities.Application.Queries.ListCapabilities
{
    /// <summary>
    /// Provides the view representation for the <see cref="Capability"/> entity.
    /// </summary>
    internal sealed class CapabilityDto : ICapability
    {
        /// <summary>
        /// Gets or sets the capability reference.
        /// </summary>
        public string CapabilityReference { get; set; }

        /// <summary>
        /// Gets or sets the version of the capability.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the name of the capability.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this instance is the criteria to form a foundation solution.
        /// </summary>
        public bool IsFoundation { get; set; }
    }
}
