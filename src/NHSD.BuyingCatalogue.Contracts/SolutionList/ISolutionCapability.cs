using System;

namespace NHSD.BuyingCatalogue.Contracts.SolutionList
{
    public interface ISolutionCapability
    {
        /// <summary>
        /// Identifier of the capability.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Name of the capability.
        /// </summary>
        string Name { get; }
    }
}
