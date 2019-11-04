using System;
using NHSD.BuyingCatalogue.Domain.Infrastructure;

namespace NHSD.BuyingCatalogue.Domain.Entities.Capabilities
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
    public class Capability : EntityBase<Guid>
    {
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
        public Capability()
        {
        }
    }
}
