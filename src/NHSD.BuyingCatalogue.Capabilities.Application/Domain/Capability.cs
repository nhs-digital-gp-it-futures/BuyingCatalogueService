using NHSD.BuyingCatalogue.Capabilities.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Capabilities.Application.Domain
{
    /// <summary>
    /// Represents a competencies for a ‘<see cref="Solution"/>’ can perform or provide eg
    /// * Mobile working
    /// * Training
    /// * Prescribing
    /// * Installation
    /// 
    /// Note that a ‘capability’ has a link to zero or one previous ‘capability’
    /// Generally, only interested in current ‘capability’
    /// </summary>
    internal class Capability
    {

        /// <summary>
        /// Capability Reference
        /// </summary>
        public string CapabilityReference { get; set; }

        /// <summary>
        /// Version of the Capability
        /// </summary>
        public string Version { get; set; }


        /// <summary>
        /// Name of the capability.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A true or false value to determine whether or not this entity is part of the criteria to make a foundation solution.
        /// </summary>
        public bool IsFoundation { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Capability"/> class.
        /// </summary>
        internal Capability(ICapabilityListResult capabilityListResult)
        {
            CapabilityReference = capabilityListResult.CapabilityReference;
            Version = capabilityListResult.Version;
            Name = capabilityListResult.Name;
            IsFoundation = capabilityListResult.IsFoundation;
        }
    }
}
