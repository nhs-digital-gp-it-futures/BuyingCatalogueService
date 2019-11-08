using System;

namespace NHSD.BuyingCatalogue.Contracts.Capability
{
    public interface ICapability
    {
        /// <summary>
        /// Identifier of the capability.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Name of the capability.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A value to determine whether or not this instance is the criteria to form a foundation solution.
        /// </summary>
        bool IsFoundation { get; }
    }
}
