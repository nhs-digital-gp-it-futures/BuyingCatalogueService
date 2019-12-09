using System;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Domain
{
    internal sealed class SolutionListItemCapability
    {
        public SolutionListItemCapability(ISolutionListResult item)
        {
            Id = item.ThrowIfNull("Capability").CapabilityId;
            Name = item.CapabilityName;
        }

        /// <summary>
        /// Identifier of the capability.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Name of the capability.
        /// </summary>
        public string Name { get; }
    }
}
