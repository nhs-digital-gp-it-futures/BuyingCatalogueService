using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence
{
    /// <summary>
    /// Defines a data contract representing the functionality for the persistence layer specific to the <see cref="ISolutionListResult"/> object.
    /// </summary>
    public interface ISolutionListRepository
    {
        /// <summary>
        /// Gets a list of <see cref="ISolutionListResult"/> objects.
        /// </summary>
        /// <returns>A list of <see cref="ISolutionListResult"/> objects.</returns>
        Task<IEnumerable<ISolutionListResult>> ListAsync(bool foundationOnly, CancellationToken cancellationToken);
    }
}
