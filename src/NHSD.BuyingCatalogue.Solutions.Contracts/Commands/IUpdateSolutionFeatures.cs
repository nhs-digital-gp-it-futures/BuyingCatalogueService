using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands
{
    public interface IUpdateSolutionFeatures
    {
        /// <summary>
        /// Features of the solution.
        /// </summary>
        IEnumerable<string> Listing { get; }
    }
}
