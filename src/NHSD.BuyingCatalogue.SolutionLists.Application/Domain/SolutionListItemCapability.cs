using System;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Domain
{
    internal sealed class SolutionListItemCapability
    {
        public SolutionListItemCapability(ISolutionListResult solutionList)
        {
            if (solutionList is null)
            {
                throw new ArgumentNullException(nameof(solutionList));
            }

            CapabilityReference = solutionList.CapabilityReference;
            Name = solutionList.CapabilityName;
        }

        /// <summary>
        /// Identifier of the capability.
        /// </summary>
        public string CapabilityReference { get; }

        /// <summary>
        /// Name of the capability.
        /// </summary>
        public string Name { get; }
    }
}
