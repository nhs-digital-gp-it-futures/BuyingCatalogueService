using System.Collections.Generic;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    /// <summary>
    /// A view representation of the <see cref="Solution"/> entity that matched a specific ID.
    /// </summary>
    public sealed class SolutionByIdViewModel
    {
        internal SolutionByIdViewModel(Solution solution)
        {
            Id = solution.Id;
            Name = solution.Name;
            MarketingData = new MarketingDataViewModel(solution);
        }

        /// <summary>
        /// Identifier of the solution.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Name of the solution.
        /// </summary>
        public string Name { get; }

        public MarketingDataViewModel MarketingData { get; }
    }
}
