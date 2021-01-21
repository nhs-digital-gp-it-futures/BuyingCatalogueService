using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands
{
    public interface IUpdateSolutionFeatures
    {
        /// <summary>
        /// Gets the features of the solution.
        /// </summary>
        IEnumerable<string> Listing { get; }
    }
}
