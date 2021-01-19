using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.SolutionLists.Application.Queries.ListSolutions;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    /// <summary>
    /// Provides the filter criteria for the <see cref="ListSolutionsQuery"/> query.
    /// </summary>
    public sealed class ListSolutionsFilterViewModel : IListSolutionsQueryData
    {
        public IEnumerable<CapabilityReferenceViewModel> Capabilities { get; } = new List<CapabilityReferenceViewModel>();

        [JsonIgnore]
        public IEnumerable<ICapabilityReference> CapabilityReferences { get => Capabilities; }

        /// <summary>
        /// Gets or sets a value indicating whether to filter to foundation solutions only.
        /// </summary>
        public bool IsFoundation { get; set; }

        public string SupplierId { get; set; }
    }
}
