using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    public sealed class UpdateSolutionFeaturesViewModel
    {
        /// <summary>
        /// Features of the solution.
        /// </summary>
        public IEnumerable<string> Listing { get; set; }
    }
}
