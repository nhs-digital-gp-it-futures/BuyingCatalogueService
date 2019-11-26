using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures
{
    public sealed class UpdateSolutionFeaturesViewModel
    {
        /// <summary>
        /// Features of the solution.
        /// </summary>
        public IEnumerable<string> Listing { get; set; }
    }
}
