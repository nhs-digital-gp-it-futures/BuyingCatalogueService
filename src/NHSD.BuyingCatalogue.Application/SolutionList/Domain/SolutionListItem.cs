using System;
using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.SolutionList.Domain
{
    internal sealed class SolutionListItem
    {
        private readonly HashSet<SolutionListItemCapability> _capabilities = new HashSet<SolutionListItemCapability>();

        /// <summary>
        /// Unique ID of the entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the solution, as displayed to a user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Summary of the solution, as displayed to a user.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>x
        /// Associated organisation.
        /// </summary>
        public SolutionListItemOrganisation Organisation { get; set; }

        /// <summary>
        /// A list of capabilities associated with the solution.
        /// </summary>
        public IReadOnlyCollection<SolutionListItemCapability> Capabilities
        {
            get
            {
                return _capabilities;
            }
        }

        /// <summary>
        /// Adds the specified capability.
        /// </summary>
        /// <param name="capability">The details of a <see cref="SolutionListItemCapability"/>.</param>
        public void AddCapability(SolutionListItemCapability capability)
        {
            if (capability is null)
            {
                throw new ArgumentNullException(nameof(capability));
            }

            _capabilities.Add(capability);
        }
    }

    internal sealed class SolutionListItemCapability
    {
        /// <summary>
        /// Identifier of the capability.
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
    }

    internal sealed class SolutionListItemOrganisation
    {
        /// <summary>
        /// Identifier of the organisation.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the organisation.
        /// </summary>
        public string Name { get; set; }
    }
}
