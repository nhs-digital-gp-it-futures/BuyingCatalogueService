using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
        public IEnumerable<ICapabilityReference> CapabilityReferences
        {
            get
            {
                return Capabilities.OfType<ICapabilityReference>().ToList();
            }
        }

        /// <summary>
        /// Filters to only foundation solutions
        /// </summary>
        public bool IsFoundation { get; set; } = false;

        public ListSolutionsFilterViewModel()
        {

        }

    }
}
