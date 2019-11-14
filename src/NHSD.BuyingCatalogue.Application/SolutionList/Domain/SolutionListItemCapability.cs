using System;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.SolutionList.Domain
{
    internal sealed class SolutionListItemCapability
    {
        public SolutionListItemCapability(ISolutionListResult item)
        {
            if (item is null)
            {
                throw new ArgumentNullException("Capability");
            }

            Id = item.CapabilityId;
            Name = item.CapabilityName;
            Description = item.CapabilityDescription;
        }

        /// <summary>
        /// Identifier of the capability.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Name of the capability.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Description of the capability.
        /// </summary>
        public string Description { get; }
    }
}
