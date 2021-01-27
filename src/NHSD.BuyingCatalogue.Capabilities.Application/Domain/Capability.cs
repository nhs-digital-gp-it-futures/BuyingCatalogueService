using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Capabilities.Application.Domain
{
    /// <summary>
    /// Represents the capabilities a solution possesses, e.g.
    /// * Mobile working
    /// * Training
    /// * Prescribing
    /// * Installation
    ///
    /// Note that a ‘capability’ has a link to zero or one previous ‘capability’.
    /// Generally, we're only interested in current ‘capability’.
    /// </summary>
    internal sealed class Capability
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Capability"/> class.
        /// </summary>
        /// <param name="capabilityListResult">The capability result.</param>
        internal Capability(ICapabilityListResult capabilityListResult)
        {
            CapabilityReference = capabilityListResult.CapabilityReference;
            Version = capabilityListResult.Version;
            Name = capabilityListResult.Name;
            IsFoundation = capabilityListResult.IsFoundation;
        }

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
        /// Gets or sets a value indicating whether or not this entity is part of the criteria to make a foundation solution.
        /// </summary>
        public bool IsFoundation { get; set; }
    }
}
