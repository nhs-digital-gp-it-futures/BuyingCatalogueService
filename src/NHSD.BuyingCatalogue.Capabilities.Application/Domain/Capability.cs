using System;
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
        /// Id of the capability.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the capability.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the capability.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A true or false value to determine whether or not this entity is part of the criteria to make a foundation solution.
        /// </summary>
        public bool IsFoundation { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Capability"/> class.
        /// </summary>
        internal Capability(ICapabilityListResult capabilityListResult)
        {
            Id = capabilityListResult.Id;
            Name = capabilityListResult.Name;
            IsFoundation = capabilityListResult.IsFoundation;
        }
    }
}
