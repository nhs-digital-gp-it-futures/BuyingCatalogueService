using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions
{
    /// <summary>
    /// Provides the view representation for the <see cref="ISolutionCapability"/> entity.
    /// </summary>
    internal sealed class SolutionCapabilityDto : ISolutionCapability
    {
        /// <summary>
        /// Gets or sets the identifier of the capability.
        /// </summary>
        public string CapabilityReference { get; set; }

        /// <summary>
        /// Gets or sets the name of the capability.
        /// </summary>
        public string Name { get; set; }
    }
}
