using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Domain.Entities.Capabilities;
using NHSD.BuyingCatalogue.Domain.Entities.Organisations;
using NHSD.BuyingCatalogue.Domain.Infrastructure;

namespace NHSD.BuyingCatalogue.Domain.Entities.Solutions
{
    /// <summary>
    /// A product and/or service provided by an ‘organisation’.
    /// </summary>
    public class Solution : EntityBase<string>
    {
        private readonly HashSet<Capability> _capabilities = new HashSet<Capability>();

        /// <summary>
        /// Name of the solution, as displayed to a user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Full description of the solution, as displayed to the user.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Summary of the solution, as displayed to a user.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Associated organisation.
        /// </summary>
        public Organisation Organisation { get; set; }

        /// <summary>
        /// A list of capabilities associated with the solution.
        /// </summary>
        public IReadOnlyCollection<Capability> Capabilities
        {
            get
            {
                return _capabilities;
            }
        }

        /// <summary>
        /// Gets or sets a blob of data representing a list of features.
        /// </summary>
        public string Features { get; set; }

        /// <summary>
        /// A link to provide more information about a solution.
        /// </summary>
        public string AboutUrl { get; set; }

        /// <summary>
        /// Status of this instance in relation to the supplier.
        /// </summary>
        public SupplierStatus SupplierStatus { get; private set; }

        public ClientApplication ClientApplication { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Solution"/> class.
        /// </summary>
        public Solution()
        {
            SupplierStatus = SupplierStatus.Draft;
        }

        /// <summary>
        /// Adds the specified capability.
        /// </summary>
        /// <param name="capability">The details of a <see cref="Capability"/>.</param>
        public void AddCapability(Capability capability)
        {
            if (capability is null)
            {
                throw new ArgumentNullException(nameof(capability));
            }

            _capabilities.Add(capability);
        }
    }
}
